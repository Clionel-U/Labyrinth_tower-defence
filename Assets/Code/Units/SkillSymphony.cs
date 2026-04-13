using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSymphony : Skill
{
    [Header("Нейросимфония")]
    public float healBoostPercent = 0.2f;  // +20% к лечению
    public float atkBoostPercent = 0.2f;   // +20% к атаке союзников

    private HealTargetInRange healComp;
    private int healBuff;

    // Отслеживаем забаффенных союзников и их бонус
    private List<EntityData> buffedAllies = new List<EntityData>();

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = true;
        maxSP = 45f;
        duration = 30f;

        healComp = GetComponent<HealTargetInRange>();
    }

    protected override void Update()
    {
        base.Update();

        // Пока скилл активен — следим за новыми союзниками в зоне
        if (!isActive) return;

        foreach (var ally in healComp.targetsInRange)
        {
            if (ally == null) continue;
            if (buffedAllies.Contains(ally)) continue; // уже забаффен

            ApplyAtkBuff(ally);
        }

        // Снимаем бафф с тех кто вышел из зоны
        // (опционально — зависит от дизайна, сейчас бафф висит до деактивации)
    }

    protected override void OnSkillActivate()
    {
        // Баффаем всех союзников в зоне
        foreach (var ally in healComp.targetsInRange)
        {
            if (ally == null) continue;
            ApplyAtkBuff(ally);
        }
    }

    protected override void OnSkillDeactivate()
    {
        // Снимаем бафф атаки со всех союзников
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