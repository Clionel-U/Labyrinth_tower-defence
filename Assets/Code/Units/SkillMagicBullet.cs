using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMagicBullet : Skill
{
    [Header("��������� ���������� ����")]
    [Range(1, 7)] public int currentBullets = 1; // ������� ������� ����
    public int attackPermanentBonus;         // ��� ����� ������ ����
    public float friendlyFireMaxHPPercent = 0.6f; // 60% ���� �� ��������

    private TargetInRange _targetList;
    private AttackTargetInRange attackComp;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerAttack; // ���������� �� ����
        activationType = ActivationType.Auto;
        hasDuration = false; // ���������� �������
        maxSP = 6f;

        _targetList = GetComponent<TargetInRange>();
        attackComp = GetComponent<AttackTargetInRange>();
    }

    public override bool CanActivate()
    {
        if (attackComp.cooldown > 0.1f) return false;

        if (currentBullets == 7) return true; // 7-� ���� ������ ����� ���������� � ��������

        if (targetList.targetsInRange.Count > 0) return true;

        return false;
    }

    protected override void OnSkillActivate()
    {
        if (currentBullets < 7)
        {
            ShootEnemy();
        }
        else
        {
            ShootAlly();
        }

        attackComp.cooldown = self.attackInterval; // ���������� ������� ����� ����� ��������

        // ������ ��������� �������� ����� ��������
        IncrementBullets();

        // ����� ����������, ������� ����� ������������
        Deactivate();
    }

    private void ShootEnemy()
    {
        // ���� ����� � ���������� ������� (DEF)
        EntityData target = null;
        int minDef = int.MaxValue;

        foreach (var enemy in targetList.targetsInRange)
        {
            if (enemy == null || enemy.entityType != EntityType.Enemy) continue;

            if (enemy.DEF < minDef)
            {
                minDef = enemy.DEF;
                target = enemy;
            }
        }

        if (target != null)
        {
            // bulletAtk = ATK * 2 * ���������� ����
            int bulletAtk = Mathf.RoundToInt(self.ATK * 2 * currentBullets);
            int finalDamage = Damage.CalculateFromAttack(bulletAtk, AtkType.Physical, target);

            target.TakeDamage(finalDamage);
            Debug.Log($"���������� ���� ({currentBullets}) �������� {target._name} �� {finalDamage} �����");
        }
    }

    private void ShootAlly()
    {
        EntityData targetAlly;

        if (_targetList.targetsInRange.Count > 0)
        {
            // ��������� ������ �������
            targetAlly = _targetList.targetsInRange[Random.Range(0, _targetList.targetsInRange.Count)];
        }
        else
        {
            // ���� ������ ��� - �������� � ����
            targetAlly = self;
        }

        // ���� �� �������� = 60% ���� �� �������� + ����� (��������� ������)
        int damage = Mathf.RoundToInt(targetAlly.maxHP * friendlyFireMaxHPPercent) + self.ATK;
        targetAlly.TakeDamage(damage);

        Debug.Log($"7-� ���� �������� �������� {targetAlly._name} �� {damage} ������� �����");
    }

    private void IncrementBullets()
    {
        currentBullets++;

        if (currentBullets > 7)
        {
            currentBullets = 1;
            // ������ ���� � �����
            self.baseATK += attackPermanentBonus;
            self.ATK += attackPermanentBonus; // ��������� � ������� �����
            Debug.Log($"���� ��������. ����� {self._name} �������� ��������� �� {attackPermanentBonus}.");
        }
    }

    protected override void OnSkillDeactivate() { }
}
