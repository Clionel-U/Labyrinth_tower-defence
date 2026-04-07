using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPotion : Skill
{
    [Header("Препарат NT-X7")]
    public float poisonDamagePercent = 0.3f; // % от ATK за каждый тик яда
    public float slowPercent = 0.5f;          // замедление на 50%
    public float effectDuration = 3f;         // длительность яда и замедления
    public float poisonTickInterval = 1f;     // интервал тика яда

    private HealTargetInRange healComp;
    private TargetInRange targetList;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerAttack;
        activationType = ActivationType.Auto;
        hasDuration = false;
        maxSP = 3f;

        healComp = GetComponent<HealTargetInRange>();
        targetList = GetComponent<TargetInRange>();
    }

    protected override void OnSkillActivate()
    {
        if (targetList.targetsInRange.Count == 0)
        {
            currentSP = 3f;
            isActive = false;
            return;
        }

        // Берём врага
        EntityData target = targetList.targetsInRange[0];

        if (target != null)
            StartCoroutine(ApplyEffects(target));

        Deactivate();
    }

    IEnumerator ApplyEffects(EntityData target)
    {
        if (target == null) yield break;

        // Применяем замедление
        float originalSpeed = target.speed;
        target.speed *= (1f - slowPercent);

        float elapsed = 0f;
        float tickTimer = 0f;

        while (elapsed < effectDuration)
        {
            if (target == null) yield break; // враг умер

            tickTimer += Time.deltaTime;
            elapsed += Time.deltaTime;

            if (tickTimer >= poisonTickInterval)
            {
                tickTimer = 0f;
                int poisonDmg = Mathf.RoundToInt(self.ATK * poisonDamagePercent);

                // Яд — магический урон, игнорирует DEF
                AtkType originalType = self.atkType;
                self.atkType = AtkType.Magical;
                int dmg = Damage.Calculate(self, target);
                target.TakeDamage(dmg);
                self.atkType = originalType;
            }

            yield return null;
        }

        // Снимаем замедление если враг ещё жив
        if (target != null)
            target.speed = originalSpeed;
    }

    protected override void OnSkillDeactivate() { }
}
