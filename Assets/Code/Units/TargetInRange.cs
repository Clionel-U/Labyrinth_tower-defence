using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRange : MonoBehaviour
{
    public List<EntityData> targetsInRange = new List<EntityData>();

    private EntityData self;

    private void Awake()
    {
        self = GetComponent<EntityData>();
    }

    private void Update()
    {
        targetsInRange.RemoveAll(t => t == null); // чистим список

        if (targetsInRange.Count == 0) return; // если нет врагов — ничего не делаем

        EntityData target = targetsInRange[0]; // выбираем цель
    }
}
