using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDiscord : Skill
{
    private readonly List<EntityData> keysCache = new List<EntityData>();

    [Header("Параметры Мистюна")]
    public float mistuneDamagePercent;
    public float mistuneTickInterval;
    public float instantDamagePercent;

    // Храним врага и время, когда ему в следующий раз нанести урон
    private readonly Dictionary<EntityData, float> activeMistunes = new Dictionary<EntityData, float>();

    // Список для удаления (чтобы не менять словарь во время перебора)
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
        atkComp.enabled = false; // Включаем только при активации ульты
    }

    protected override void Update()
    {
        base.Update(); // Важно для SP и таймера скилла

        HandleMistuneLogic();
    }

    private void HandleMistuneLogic()
    {
        int mistuneAtk = Mathf.RoundToInt(self.ATK * mistuneDamagePercent);

        // 1. ОПРЕДЕЛЯЕМ ПУЛ ВРАГОВ
        // Если скилл активен — берем всех. Если нет — только тех, кто в радиусе.
        var potentialTargets = isActive ? AllEnemies.allEnemies : _targetList.targetsInRange;

        // 2. ДОБАВЛЯЕМ НОВЫХ ВРАГОВ
        foreach (var enemy in potentialTargets)
        {
            if (enemy == null) continue;

            if (!activeMistunes.ContainsKey(enemy))
            {
                // Засекаем личное время первого тика (можно сделать мгновенно или через интервал)
                activeMistunes.Add(enemy, Time.time + mistuneTickInterval);
            }
        }

        // 3. ТИКАЕМ УРОНОМ И ПРОВЕРЯЕМ ВЫХОД
        toRemove.Clear();
        keysCache.Clear();

        foreach (var key in activeMistunes.Keys) keysCache.Add(key);

        foreach (var enemy in keysCache)
        {
            // Условие удаления: враг умер ИЛИ (скилл не активен И враг вышел из зоны)
            if (enemy == null || (!isActive && !_targetList.targetsInRange.Contains(enemy)))
            {
                toRemove.Add(enemy);
                continue;
            }

            // ПРОВЕРКА ТАЙМЕРА (Индивидуальный тик)
            if (Time.time >= activeMistunes[enemy])
            {
                int damage = Damage.CalculateFromAttack(mistuneAtk, AtkType.Magical, enemy);
                enemy.TakeDamage(damage);
                // Назначаем время следующего тика для ЭТОГО врага
                activeMistunes[enemy] = Time.time + mistuneTickInterval;
            }
        }

        // Удаляем "отвалившихся" врагов
        foreach (var enemy in toRemove)
        {
            activeMistunes.Remove(enemy);
        }
    }

    protected override void OnSkillActivate()
    {
        if (atkComp != null) atkComp.enabled = true;

        // Мгновенный урон по всем при прожатии
        int instantDmgAtk = Mathf.RoundToInt(self.ATK * instantDamagePercent);
        for (int i = AllEnemies.allEnemies.Count - 1; i >= 0; i--)
        {
            if (AllEnemies.allEnemies[i] != null)
            {
                int instantDmg = Damage.CalculateFromAttack(instantDmgAtk, AtkType.Magical, AllEnemies.allEnemies[i]);
                AllEnemies.allEnemies[i].TakeDamage(instantDmg);
            }
        }

        // При активации мы не чистим словарь, так как враги в зоне просто 
        // продолжат тикать по своим таймерам, а остальные добавятся в HandleMistuneLogic
    }

    protected override void OnSkillDeactivate()
    {
        if (atkComp != null) atkComp.enabled = false;

        // Когда ульта кончилась, нужно мгновенно убрать из словаря всех, 
        // кто НЕ находится в зоне пассивки
        toRemove.Clear();
        foreach (var enemy in activeMistunes.Keys)
        {
            if (!_targetList.targetsInRange.Contains(enemy))
                toRemove.Add(enemy);
        }
        foreach (var enemy in toRemove) activeMistunes.Remove(enemy);
    }
}