using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTurret : Skill
{
    private UnitBlock blockComp;

    [Header("Зоны атаки")]
    public GameObject normalRange;   // обычная зона — назначить в инспекторе
    public GameObject skillRange;    // зона скилла — назначить в инспекторе

    private Range normalRangeComp;
    private Range skillRangeComp;
    private BoxCollider2D skillRangeCollider;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerAttack;
        activationType = ActivationType.Manual;
        hasDuration = false;
        canToggleOff = true;
        maxSP = 10f;

        blockComp = GetComponent<UnitBlock>();
        normalRangeComp = normalRange.GetComponent<Range>();
        skillRangeComp = skillRange.GetComponent<Range>();
        skillRangeCollider = skillRange.GetComponent<BoxCollider2D>();

        // Зона скилла изначально выключена
        skillRange.SetActive(false);
    }

    protected override void OnSkillActivate()
    {
        // Принудительно освобождаем всех заблокированных врагов
        if (blockComp != null)
        {
            ForceUnblockAll();
            blockComp.enabled = false;
        }

        normalRangeComp.targetList.targetsInRange.Clear();

        // Меняем зоны
        normalRange.SetActive(false);
        skillRange.SetActive(true);

        // Включаем атаку по воздуху и земле
        skillRangeComp.canAttackGround = true;
        skillRangeComp.canAttackAir = true;

        RefreshTargetList(skillRangeComp, skillRangeCollider);
    }

    protected override void OnSkillDeactivate()
    {
        // Возвращаем блокировку
        if (blockComp != null)
            blockComp.enabled = true;

        skillRangeComp.targetList.targetsInRange.Clear();

        // Возвращаем зоны
        skillRange.SetActive(false);
        normalRange.SetActive(true);

        skillRangeComp.canAttackGround = true;
        skillRangeComp.canAttackAir = false;

        RefreshTargetList(normalRangeComp, normalRange.GetComponent<BoxCollider2D>());
    }

    void ForceUnblockAll()
    {
        foreach (var enemy in FindObjectsOfType<EnemyBlock>())
        {
            if (enemy.blocker == self)
                blockComp.Unblock(enemy.GetComponent<EntityData>());
        }
    }

    void RefreshTargetList(Range rangeComp, BoxCollider2D col)
    {
        var targets = rangeComp.targetList.targetsInRange;

        Vector2 center = col.transform.position + (Vector3)col.offset;
        Vector2 size = col.size;
        float angle = col.transform.eulerAngles.z;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, angle);

        foreach (var c in colliders)
        {
            EntityData entity = c.GetComponent<EntityData>();
            if (entity == null) continue;
            if (entity.entityType != EntityType.Enemy) continue;
            if (!rangeComp.CanTarget(entity)) continue;
            if (!targets.Contains(entity))
                targets.Add(entity);
        }
    }
}
