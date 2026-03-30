using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
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

//public Vector3 GetCellCenter(Vector3 worldPos)
//{
//    Vector3Int cell;
//    if (IsGround(worldPos))
//    {
//        cell = groundTilemap.WorldToCell(worldPos);
//        return groundTilemap.GetCellCenterWorld(cell);
//    }
//    else if (IsHighGround(worldPos))
//    {
//        cell = highGroundTilemap.WorldToCell(worldPos);
//        return highGroundTilemap.GetCellCenterWorld(cell);
//    }
//    else if (IsOther(worldPos))
//    {
//        cell = otherTilemap.WorldToCell(worldPos);
//        return otherTilemap.GetCellCenterWorld(cell);
//    }
//    return Vector3.zero;
//}

//public TileType GetTileType(Vector3 worldPos)
//{
//    Vector3Int cell = groundTilemap.WorldToCell(worldPos);

//    if (groundTilemap.HasTile(cell)) return TileType.Ground;
//    if (highGroundTilemap.HasTile(cell)) return TileType.HighGround;
//    if (otherTilemap.HasTile(cell)) return TileType.Other;

//    return TileType.None;
//}

//public enum TileType
//{
//    None,
//    Ground,
//    HighGround,
//    Other
//    //Road
//}


//второй вариант
//public Tilemap GroundTilemap;
//public Tilemap HighGroundTilemap;
//public Tilemap NoneTilemap;

//public Vector3 GetCellCenter(Vector3 worldPosition)
//{
//    Vector3Int cell = GroundTilemap.WorldToCell(worldPosition);
//    return GroundTilemap.GetCellCenterWorld(cell);
//}

//public bool IsGroundCell(Vector3 worldPosition)
//{
//    Vector3Int cell = GroundTilemap.WorldToCell(worldPosition);
//    return GroundTilemap.HasTile(cell);
//}

//public bool IsHighGroundCell(Vector3 worldPosition)
//{
//    Vector3Int cell = HighGroundTilemap.WorldToCell(worldPosition);
//    return HighGroundTilemap.HasTile(cell);
//}

//public bool IsBlocked(Vector3 worldPosition) //IsNoneCell
//{
//    Vector3Int cell = NoneTilemap.WorldToCell(worldPosition);
//    return NoneTilemap.HasTile(cell);
//}

// первый вариант
//public Grid grid;

//public Tilemap NoneTilemap;
//public Tilemap GroundTilemap;
//public Tilemap HighGroundTilemap;
//public Tilemap RoadTilemap;

//public TileType GetTileType(Vector3 worldPosition)
//{
//    Vector3Int cell = grid.WorldToCell(worldPosition);

//    if (GroundTilemap.HasTile(cell))
//        return TileType.Ground;

//    if (HighGroundTilemap.HasTile(cell))
//        return TileType.HighGround;

//    //if (RoadTilemap.HasTile(cell))
//    //    return TileType.Road;

//    return TileType.None;
//}