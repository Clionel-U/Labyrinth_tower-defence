using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTargets : MonoBehaviour
{
    public List<EntityData> targetsInRange = new List<EntityData>();

    private void Update()
    {
        targetsInRange.RemoveAll(t => t == null); // чистим список
    }
}
