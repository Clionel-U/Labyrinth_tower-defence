using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public Camera cam;
    public GridManager grid;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;

            if (grid.IsHighGround(mouseWorld))
            {
                Debug.Log("HighGround - ставим дальника");
            }
            else if (grid.IsGround(mouseWorld))
            {
                Debug.Log("Ground - ставим мили");
            }
            //else if (grid.IsOther(mouseWorld))
            //{
            //    Debug.Log("Spawn/Exit");
            //}
            else
            {
                Debug.Log("Ќельз€ ставить");
            }
        }
    }
}
