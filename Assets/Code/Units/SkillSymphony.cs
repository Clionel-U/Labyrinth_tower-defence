using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSymphony : Skill
{
    [Header("�������������")]
    public float healBoostPercent = 0.2f;  // +20% � �������
    public float atkBoostPercent = 0.2f;   // +20% � ����� ���������

    private HealTargetInRange healComp;
    private int healBuff;

    // ����������� ����������� ��������� � �� �����
    private List<EntityData> buffedAllies = new List<EntityData>();

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = true;
        maxSP = 40f;
        duration = 30f;

        healComp = GetComponent<HealTargetInRange>();
    }

    protected override void Update()
    {
        base.Update();

        // ���� ����� ������� � ������ �� ������ ���������� � ����
        if (!isActive) return;

        foreach (var ally in healComp.targetsInRange)
        {
            if (ally == null) continue;
            if (buffedAllies.Contains(ally)) continue; // ��� ��������

            ApplyAtkBuff(ally);
        }

        // ������� ���� � ��� ��� ����� �� ����
        // (����������� � ������� �� �������, ������ ���� ����� �� �����������)
    }

    protected override void OnSkillActivate()
    {
        // ������� ���� ��������� � ����
        foreach (var ally in healComp.targetsInRange)
        {
            if (ally == null) continue;
            ApplyAtkBuff(ally);
        }
    }

    protected override void OnSkillDeactivate()
    {
        // ������� ���� ����� �� ���� ���������
        foreach (var ally in buffedAllies)
        {
            if (ally == null) continue;
            int buff = Mathf.RoundToInt(ally.baseATK * atkBoostPercent);
            ally.ATK -= buff;
            ally.healBonus -= healBoostPercent;
        }

        buffedAllies.Clear();
    }

    void ApplyAtkBuff(EntityData ally)
    {
        int buff = Mathf.RoundToInt(ally.baseATK * atkBoostPercent);
        ally.ATK += buff;
        ally.healBonus += healBoostPercent;
        buffedAllies.Add(ally);
    }
}