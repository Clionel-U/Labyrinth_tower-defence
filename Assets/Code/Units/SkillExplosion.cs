using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExplosion : Skill
{
    [Header("Параметры Подрыва")]
    public float explosionDamageMultiplier = 3f;
    public float stopDuration = 3f;
    public AtkType damageType = AtkType.Magical;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = false;
        maxSP = 20f;
    }

    // Проверка: есть ли кого подрывать?
    public override bool CanActivate()
    {
        if (targetList.targetsInRange.Count > 0) return true;
        return false;
    }

    protected override void OnSkillActivate()
    {
        Explode();

        // Скилл мгновенный, завершаем его
        Deactivate();
    }

    private void Explode()
    {
        // Рассчитываем урон один раз
        int ExplodeAtk = Mathf.RoundToInt(self.ATK * explosionDamageMultiplier);

        foreach (var enemy in targetList.targetsInRange)
        {
            if (enemy == null) continue;

            // 1. Наносим урон
            int finalDamage = Damage.CalculateFromAttack(ExplodeAtk, damageType, enemy);
            enemy.TakeDamage(finalDamage);

            // 2. Останавливаем врага
            var movement = enemy.GetComponent<EnemyMove>();
            if (movement != null)
            {
                movement.StopEnemy(stopDuration);
            }

            Debug.Log($"Враг {enemy._name} подорван. Урон: {finalDamage}, Стан: {stopDuration}с.");
        }
    }
}
