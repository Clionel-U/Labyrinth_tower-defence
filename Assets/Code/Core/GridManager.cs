using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    void Awake() => Instance = this;

    public Tilemap groundTilemap;
    public Tilemap highGroundTilemap;
    public Tilemap otherTilemap;
    [SerializeField] public HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    public bool IsGround(Vector3 worldPos)
    {
        Vector3Int cell = groundTilemap.WorldToCell(worldPos);
        return groundTilemap.HasTile(cell);
    }

    public bool IsHighGround(Vector3 worldPos)
    {
        Vector3Int cell = highGroundTilemap.WorldToCell(worldPos);
        return highGroundTilemap.HasTile(cell);
    }

    public bool IsOther(Vector3 worldPos)
    {
        Vector3Int cell = otherTilemap.WorldToCell(worldPos);
        return otherTilemap.HasTile(cell);
    }

    public Vector3 GetCellCenter(Vector3 worldPos)
    {
        Vector3Int cell;
        cell = groundTilemap.WorldToCell(worldPos);
        if (groundTilemap.HasTile(cell))
            return groundTilemap.GetCellCenterWorld(cell);
        if (highGroundTilemap.HasTile(cell))
            return highGroundTilemap.GetCellCenterWorld(cell);
        if (otherTilemap.HasTile(cell))
            return otherTilemap.GetCellCenterWorld(cell);
        return Vector3.zero;
    }
}