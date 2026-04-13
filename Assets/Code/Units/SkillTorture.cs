using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTorture : Skill
{
    [Header("Всемирная пытка")]
    public float damagePercent = 0.6f; // 60% от ATK за каждый хит
    public int hitCount = 3;           // количество инстанций урона
    public int skillAtk;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = false;
        maxSP = 15f;
    }

    public override bool CanActivate()
    {
        if (AllEnemies.allEnemies.Count == 0)
        {
            Debug.Log("Нет врагов для пытки");
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

        // Находим всех врагов на сцене
        EntityData[] allEnemies = FindObjectsOfType<EntityData>();

        foreach (var enemy in allEnemies)
        {
            if (enemy.entityType != EntityType.Enemy) continue;

            for (int i = 0; i < hitCount; i++)
            {
                if (enemy == null) break; // враг мог умереть от предыдущего хита
                int dmg = Damage.CalculateFromAttack(skillAtk, AtkType.Physical, enemy);
                enemy.TakeDamage(dmg);
            }
        }

        // Одноразовый скилл — сразу деактивируем
        Deactivate();
    }

    protected override void OnSkillDeactivate() { }
}