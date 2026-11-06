using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTorture : Skill
{
    [Header("��������� �����")]
    public float damagePercent = 0.6f; // 60% �� ATK �� ������ ���
    public int hitCount = 3;           // ���������� ��������� �����
    private int skillAtk;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = false;
        maxSP = 25f;
    }

    public override bool CanActivate()
    {
        if (AllEnemies.allEnemies.Count == 0)
        {
            Debug.Log("��� ������ ��� �����");
            return false;
        }
        else
        {
            return base.CanActivate();
        }
    }

    protected override void OnSkillActivate()
    {
        skillAtk = Mathf.RoundToInt(self.ATK * damagePercent);

        // ������� ���� ������ �� �����
        EntityData[] allEnemies = FindObjectsOfType<EntityData>();

        foreach (var enemy in allEnemies)
        {
            if (enemy.entityType != EntityType.Enemy) continue;

            for (int i = 0; i < hitCount; i++)
            {
                if (enemy == null) break; // ���� ��� ������� �� ����������� ����
                int dmg = Damage.CalculateFromAttack(skillAtk, AtkType.Physical, enemy);
                enemy.TakeDamage(dmg);
            }
        }

        // ����������� ����� � ����� ������������
        Deactivate();
    }

    protected override void OnSkillDeactivate() { }
}