using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTurret : Skill
{
    [Header("����")]
    public GameObject block;
    private UnitBlock blockComp;

    [Header("���� �����")]
    public GameObject normalRange;   // ������� ���� � ��������� � ����������
    public GameObject skillRange;    // ���� ������ � ��������� � ����������

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

        blockComp = block.GetComponent<UnitBlock>();
        normalRangeComp = normalRange.GetComponent<Range>();
        skillRangeComp = skillRange.GetComponent<Range>();
        skillRangeCollider = skillRange.GetComponent<BoxCollider2D>();

        // ���� ������ ���������� ���������
        skillRange.SetActive(false);
    }

    protected override void OnSkillActivate()
    {
        // ������������� ����������� ���� ��������������� ������
        if (block != null)
        {
            ForceUnblockAll();
            block.SetActive(false);
        }

        normalRangeComp.targetList.targetsInRange.Clear();

        // ������ ����
        normalRange.SetActive(false);
        skillRange.SetActive(true);

        RefreshTargetList(skillRangeComp, skillRangeCollider);
    }

    protected override void OnSkillDeactivate()
    {
        // ���������� ����������
        if (block != null)
            block.SetActive(true);

        skillRangeComp.targetList.targetsInRange.Clear();

        // ���������� ����
        skillRange.SetActive(false);
        normalRange.SetActive(true);

        RefreshTargetList(normalRangeComp, normalRange.GetComponent<BoxCollider2D>());
    }

    void ForceUnblockAll()
    {
        foreach (var enemy in FindObjectsOfType<EnemyBlock>())
        {
            if (enemy.blocker == self)
                blockComp.Unblock(enemy.GetComponent<EntityData>(), enemy);
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
