using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDiscord : Skill
{
    private readonly List<EntityData> keysCache = new List<EntityData>();

    [Header("��������� �������")]
    public float mistuneDamagePercent;
    public float mistuneTickInterval;
    public float instantDamagePercent;

    // ������ ����� � �����, ����� ��� � ��������� ��� ������� ����
    private readonly Dictionary<EntityData, float> activeMistunes = new Dictionary<EntityData, float>();

    // ������ ��� �������� (����� �� ������ ������� �� ����� ��������)
    private readonly List<EntityData> toRemove = new List<EntityData>();

    private AttackTargetInRange atkComp;
    private TargetInRange _targetList;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = true;
        duration = 30f;
        maxSP = 40f;

        atkComp = GetComponent<AttackTargetInRange>();
        _targetList = GetComponent<TargetInRange>();
        atkComp.enabled = false; // �������� ������ ��� ��������� �����
    }

    protected override void Update()
    {
        base.Update(); // ����� ��� SP � ������� ������

        HandleMistuneLogic();
    }

    private void HandleMistuneLogic()
    {
        int mistuneAtk = Mathf.RoundToInt(self.ATK * mistuneDamagePercent);

        // 1. ���������� ��� ������
        // ���� ����� ������� � ����� ����. ���� ��� � ������ ���, ��� � �������.
        var potentialTargets = isActive ? AllEnemies.allEnemies : _targetList.targetsInRange;

        // 2. ��������� ����� ������
        foreach (var enemy in potentialTargets)
        {
            if (enemy == null) continue;

            if (!activeMistunes.ContainsKey(enemy))
            {
                // �������� ������ ����� ������� ���� (����� ������� ��������� ��� ����� ��������)
                activeMistunes.Add(enemy, Time.time + mistuneTickInterval);
            }
        }

        // 3. ������ ������ � ��������� �����
        toRemove.Clear();
        keysCache.Clear();

        foreach (var key in activeMistunes.Keys) keysCache.Add(key);

        foreach (var enemy in keysCache)
        {
            // ������� ��������: ���� ���� ��� (����� �� ������� � ���� ����� �� ����)
            if (enemy == null || (!isActive && !_targetList.targetsInRange.Contains(enemy)))
            {
                toRemove.Add(enemy);
                continue;
            }

            // �������� ������� (�������������� ���)
            if (Time.time >= activeMistunes[enemy])
            {
                int damage = Damage.CalculateFromAttack(mistuneAtk, AtkType.Magical, enemy);
                enemy.TakeDamage(damage);
                // ��������� ����� ���������� ���� ��� ����� �����
                activeMistunes[enemy] = Time.time + mistuneTickInterval;
            }
        }

        // ������� "������������" ������
        foreach (var enemy in toRemove)
        {
            activeMistunes.Remove(enemy);
        }
    }

    protected override void OnSkillActivate()
    {
        if (atkComp != null) atkComp.enabled = true;

        // ���������� ���� �� ���� ��� ��������
        int instantDmgAtk = Mathf.RoundToInt(self.ATK * instantDamagePercent);
        for (int i = AllEnemies.allEnemies.Count - 1; i >= 0; i--)
        {
            if (AllEnemies.allEnemies[i] != null)
            {
                int instantDmg = Damage.CalculateFromAttack(instantDmgAtk, AtkType.Magical, AllEnemies.allEnemies[i]);
                AllEnemies.allEnemies[i].TakeDamage(instantDmg);
            }
        }

        // ��� ��������� �� �� ������ �������, ��� ��� ����� � ���� ������ 
        // ��������� ������ �� ����� ��������, � ��������� ��������� � HandleMistuneLogic
    }

    protected override void OnSkillDeactivate()
    {
        if (atkComp != null) atkComp.enabled = false;

        // ����� ����� ���������, ����� ��������� ������ �� ������� ����, 
        // ��� �� ��������� � ���� ��������
        toRemove.Clear();
        foreach (var enemy in activeMistunes.Keys)
        {
            if (!_targetList.targetsInRange.Contains(enemy))
                toRemove.Add(enemy);
        }
        foreach (var enemy in toRemove) activeMistunes.Remove(enemy);
    }
}