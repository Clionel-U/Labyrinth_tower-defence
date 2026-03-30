using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public GameObject highlightPrefab;
    public GridManager grid;

    public List<GameObject> highlights = new List<GameObject>();

    public void ShowHighlights(UnitType type)
    {
        Clear();

        for (int x = -10; x < 10; x++)
        {
            for (int y = -5; y < 5; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);

                if ((type == UnitType.Ranged  && grid.IsHighGround(pos)) || (type == UnitType.Melee && grid.IsGround(pos)))
                {
                    pos = grid.GetCellCenter(pos);
                    if (!grid.occupiedPositions.Contains(pos))
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
