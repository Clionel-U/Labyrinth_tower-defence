using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRange : MonoBehaviour
{
    public BossTargets targetList;
    public EntityType targets = EntityType.Unit;

    private void OnEnable() //при активации объекта, очищаем список врагов в радиусе и устанавливаем тег для определения врагов
    {
        targetList.targetsInRange.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityData entity = other.GetComponent<EntityData>();
        if (entity == null || entity.entityType != targets) return;

        if (!targetList.targetsInRange.Contains(entity))
        {
            targetList.targetsInRange.Add(entity);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        EntityData entity = other.GetComponent<EntityData>();
        if (entity == null || entity.entityType != targets) return;

        targetList.targetsInRange.Remove(entity);
    }
}
