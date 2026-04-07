using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealRange : MonoBehaviour
{
    public HealTargetInRange targetList;
    private EntityData self;

    public EntityType targets = EntityType.Unit;

    private void OnEnable()
    {
        targetList.targetsInRange.Clear();
        self = GetComponentInParent<EntityData>();
        targetList.targetsInRange.Add(self);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityData entity = other.GetComponent<EntityData>();
        if (entity == null) return;
        if (entity.entityType != targets) return;

        if (!targetList.targetsInRange.Contains(entity))
            targetList.targetsInRange.Add(entity);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EntityData entity = other.GetComponent<EntityData>();
        if (entity == null) return;

        targetList.targetsInRange.Remove(entity);
    }
}
