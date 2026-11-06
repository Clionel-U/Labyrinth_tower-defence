using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPotion : Skill
{
    [Header("�������� NT-X7")]
    public float poisonDamagePercent = 0.4f; // % �� ATK �� ������ ��� ���
    public float slowPercent = 0.5f;          // ���������� �� 50%
    public float effectDuration;         // ������������ ��� � ����������
    public float poisonTickInterval;     // �������� ���� ���

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
        // ���� �����
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
            if (target == null) yield break; // ���� ����

            tickTimer += Time.deltaTime;
            elapsed += Time.deltaTime;

            if (tickTimer >= poisonTickInterval)
            {
                tickTimer = 0f;
                
                // �� � ���������� ����
                int dmg = Damage.CalculateFromAttack(poisonAtk, AtkType.Magical, target);
                target.TakeDamage(dmg);
            }

            yield return null;
        }

        // ������� ���������� ���� ���� ��� ���
        if (target != null && !target.isBoss)
            target.speed = originalSpeed;
    }

    protected override void OnSkillDeactivate() { }
}
