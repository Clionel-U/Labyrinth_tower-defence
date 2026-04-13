using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPotion : Skill
{
    [Header("ѕрепарат NT-X7")]
    public float poisonDamagePercent = 0.4f; // % от ATK за каждый тик €да
    public float slowPercent = 0.5f;          // замедление на 50%
    public float effectDuration;         // длительность €да и замедлени€
    public float poisonTickInterval;     // интервал тика €да

    private HealTargetInRange healComp;
    private TargetInRange _targetList;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerAttack;
        activationType = ActivationType.Auto;
        hasDuration = false;
        maxSP = 3f;

        healComp = GetComponent<HealTargetInRange>();
        _targetList = GetComponent<TargetInRange>();
    }

    public override bool CanActivate()
    {
        if (_targetList.targetsInRange.Count != 0 && healComp.cooldown < 0.1f)
        {
            healComp.cooldown = self.attackInterval; 
            return base.CanActivate();
        }
        else
        {
            return false;
        }
    }

    protected override void OnSkillActivate()
    {
        // ЅерЄм врага
        EntityData target = _targetList.targetsInRange[0];

        if (target != null)
            StartCoroutine(ApplyEffects(target));

        Deactivate();
    }

    IEnumerator ApplyEffects(EntityData target)
    {
        if (target == null) yield break;

        int poisonAtk = Mathf.RoundToInt(self.ATK * poisonDamagePercent);
        float originalSpeed = target.speed;

        if (!target.isBoss)
        {
            target.speed *= (1f - slowPercent);
        }
            
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
                
                // яд Ч магический урон
                int dmg = Damage.CalculateFromAttack(poisonAtk, AtkType.Magical, target);
                target.TakeDamage(dmg);
            }

            yield return null;
        }

        // —нимаем замедление если враг ещЄ жив
        if (target != null && !target.isBoss)
            target.speed = originalSpeed;
    }

    protected override void OnSkillDeactivate() { }
}
