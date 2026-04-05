using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public string pathName;
    public int pathID;
    public List<Transform> points = new List<Transform>();

    public Transform GetPoint(int index)
    {
        if (index < 0 || index >= points.Count) return null;
        return points[index];
    }

    public int Length => points.Count;
}
