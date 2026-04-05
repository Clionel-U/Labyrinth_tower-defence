using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public static TileHighlighter Instance;
    void Awake() => Instance = this;

    public GameObject highlightPrefab;

    public List<GameObject> highlights = new List<GameObject>();

    public void ShowHighlights(UnitType type)
    {
        Clear();

        for (int x = -11; x < 7; x++)
        {
            for (int y = -7; y < 5; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);

                if ((type == UnitType.Ranged  && GridManager.Instance.IsHighGround(pos)) || (type == UnitType.Melee && GridManager.Instance.IsGround(pos)))
                {
                    pos = GridManager.Instance.GetCellCenter(pos);
                    if (!GridManager.Instance.occupiedPositions.Contains(pos))
                    {
                        GameObject h = Instantiate(highlightPrefab, pos, Quaternion.identity);
                        highlights.Add(h);
                    }
                }
            }
        }
    }

    public void Clear()
    {
        foreach (var h in highlights)
            Destroy(h);

        highlights.Clear();
    }
}
