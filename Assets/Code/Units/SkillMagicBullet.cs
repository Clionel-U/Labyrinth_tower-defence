using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMagicBullet : Skill
{
    [Header("Параметры Магических пуль")]
    [Range(1, 7)] public int currentBullets = 1; // Текущий счетчик пуль
    public int attackPermanentBonus;         // Тот самый вечный бафф
    public float friendlyFireMaxHPPercent = 0.6f; // 60% макс ХП союзника

    private TargetInRange _targetList;
    private AttackTargetInRange attackComp;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerAttack; // Заряжается от атак
        activationType = ActivationType.Auto;
        hasDuration = false; // Мгновенный выстрел
        maxSP = 6f;

        _targetList = GetComponent<TargetInRange>();
        attackComp = GetComponent<AttackTargetInRange>();
    }

    public override bool CanActivate()
    {
        if (attackComp.cooldown > 0.1f) return false;

        if (currentBullets == 7) return true; // 7-я пуля всегда может выстрелить в союзника

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

        attackComp.cooldown = self.attackInterval; // Сбрасываем кулдаун атаки после выстрела

        // Логика изменения счетчика ПОСЛЕ выстрела
        IncrementBullets();

        // Скилл мгновенный, поэтому сразу деактивируем
        Deactivate();
    }

    private void ShootEnemy()
    {
        // Ищем врага с наименьшей защитой (DEF)
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
            // bulletAtk = ATK * 2 * количество пуль
            int bulletAtk = Mathf.RoundToInt(self.ATK * 2 * currentBullets);
            int finalDamage = Damage.CalculateFromAttack(bulletAtk, AtkType.Physical, target);

            target.TakeDamage(finalDamage);
            Debug.Log($"Магическая пуля ({currentBullets}) поразила {target._name} на {finalDamage} урона");
        }
    }

    private void ShootAlly()
    {
        EntityData targetAlly;

        if (_targetList.targetsInRange.Count > 0)
        {
            // Случайный другой союзник
            targetAlly = _targetList.targetsInRange[Random.Range(0, _targetList.targetsInRange.Count)];
        }
        else
        {
            // Если никого нет - стреляет в себя
            targetAlly = self;
        }

        // Урон по союзнику = 60% макс хп союзника + атака (Игнорируя защиту)
        int damage = Mathf.RoundToInt(targetAlly.maxHP * friendlyFireMaxHPPercent) + self.ATK;
        targetAlly.TakeDamage(damage);

        Debug.Log($"7-я пуля поразила союзника {targetAlly._name} на {damage} чистого урона");
    }

    private void IncrementBullets()
    {
        currentBullets++;

        if (currentBullets > 7)
        {
            currentBullets = 1;
            // Вечный бафф к атаке
            self.baseATK += attackPermanentBonus;
            self.ATK += attackPermanentBonus; // Обновляем и текущую атаку
            Debug.Log($"Цикл завершен. Атака {self._name} навсегда увеличена на {attackPermanentBonus}.");
        }
    }

    protected override void OnSkillDeactivate() { }
}
