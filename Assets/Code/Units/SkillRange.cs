using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    public TargetInRange targetList;
    public EntityType targets;

    public bool canAttackGround = true;
    public bool canAttackAir = true;

    private void OnEnable() //при активации объекта, очищаем список врагов в радиусе и устанавливаем тег для определения врагов
    {
        targetList.targetsInRange.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityData entity = other.GetComponent<EntityData>();
        if (entity == null || entity.entityType != targets) return;
        if (!CanTarget(entity)) return;

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

    public bool CanTarget(EntityData entity)
    {
        if (entity.entityType == EntityType.Enemy)
        {
            if (entity.enemyType == EnemyType.Ground && !canAttackGround) return false;
            if (entity.enemyType == EnemyType.Air && !canAttackAir) return false;
        }
        return true;
    }
}
