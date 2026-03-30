using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public GameObject previewUnit;

    public void RotateUp()
    {
        previewUnit.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void RotateDown()
    {
        previewUnit.transform.rotation = Quaternion.Euler(0, 0, 180);
    }
    public void RotateLeft()
    {
        previewUnit.transform.rotation = Quaternion.Euler(0, 0, 90);
    }
    public void RotateRight()
    {
        previewUnit.transform.rotation = Quaternion.Euler(0, 0, -90);
    }

}
