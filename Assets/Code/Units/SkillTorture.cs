using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTorture : Skill
{
    [Header("Всемирная пытка")]
    public float damagePercent = 0.6f; // 60% от ATK за каждый хит
    public int hitCount = 3;           // количество инстанций урона

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = false;
        maxSP = 15f;
    }

    protected override void OnSkillActivate()
    {
        // Сохраняем оригинальный ATK
        int originalATK = self.ATK;

        // Скилл всегда физический
        self.atkType = AtkType.Physical;
        self.ATK = Mathf.RoundToInt(originalATK * damagePercent);

        // Находим всех врагов на сцене
        EntityData[] allEnemies = FindObjectsOfType<EntityData>();

        foreach (var enemy in allEnemies)
        {
            if (enemy.entityType != EntityType.Enemy) continue;

            for (int i = 0; i < hitCount; i++)
            {
                if (enemy == null) break; // враг мог умереть от предыдущего хита
                int dmg = Damage.Calculate(self, enemy);
                enemy.TakeDamage(dmg);
            }
        }

        // Восстанавливаем оригинальные значения
        self.ATK = originalATK;

        // Одноразовый скилл — сразу деактивируем
        Deactivate();
    }

    protected override void OnSkillDeactivate() { }
}