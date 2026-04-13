using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkills : MonoBehaviour
{
    private EntityData self;
    private BossCore core;

    public AttackTargetInRange atkComp;
    public TargetInRange EnemiesList;
    public BossTargets UnitsList;

    private void Awake()
    {
        self = GetComponent<EntityData>();
        core = GetComponent<BossCore>();
        atkComp = GetComponent<AttackTargetInRange>();
        EnemiesList = GetComponent<TargetInRange>();
        UnitsList = GetComponent<BossTargets>();

        core.Stage2 += Stage2Transition;
        atkComp.onAttack += HeartBreakInc;
    }

    [Header("Гепарин с Варфарином")]
    public int bleedDamage;
    public float bleedTickInterval;
    public float bleedDuration;
    public float bleedInterval;
    private Dictionary<EntityData, Coroutine> activeBleeds = new Dictionary<EntityData, Coroutine>();

    private float bleedTimer;


    [Header("Адреналин")]
    public float speedBuffMultiplier;
    public float atkBuffMultiplier;
    public float buffDuration;
    public float buffInterval;
    private Dictionary<EntityData, Coroutine> activeAdrenalines = new Dictionary<EntityData, Coroutine>();

    private float adrenalineTimer;

    [Header("Разбиватель сердец")]
    [Range(0, 4)] public int attacksCount = 0;
    public float atkMultiplier = 3f;

    [Header("Перераспределение любви")]
    public int auraTickAtk;
    public float auraTickInterval;

    private float auraTickTimer;

    [Header("Любовь убивает")]
    public int loveStacks = 0;
    public int loveMax = 100;
    public float hpTreshold;
    private float lastHpCheck;
    public int explosionAtkMultiplier;


    private void Update()
    {
        if (core.currentStage == 1)
        {
            HandleSkillsStage1();
        }
        else if (core.currentStage == 2)
        {
            // 1. Аура урона и Адреналина работают постоянно
            LoveRedistribution();
            AdrenalineAura();

            // 2. Проверка условия: потеря hpTreshold ХП
            // Если босс потерял больше или равно hpTreshold от своего текущего здоровья с момента последней проверки
            if (lastHpCheck - self.HP >= hpTreshold && hpTreshold > 0)
            {
                LoveKills();
                lastHpCheck = lastHpCheck - hpTreshold; // Обновляем точку отсчета
            }
        }
    }

    private void HandleSkillsStage1()
    {
        // 1. Логика Гепарина и Варфарина
        bleedTimer += Time.deltaTime;
        if (bleedTimer >= bleedInterval)
        {
            // Ищем цель среди юнитов, у которой нет кровотечения
            foreach (var unit in UnitsList.targetsInRange)
            {
                if (unit != null && !activeBleeds.ContainsKey(unit))
                {
                    GepAndWarf(unit);
                    bleedTimer = 0; // Сбрасываем таймер только если нашли цель
                    break;
                }
            }
        }

        // 2. Логика Адреналина
        adrenalineTimer += Time.deltaTime;
        if (adrenalineTimer >= buffInterval)
        {
            int buffedCount = 0;
            foreach (var enemy in EnemiesList.targetsInRange)
            {
                if (enemy != null && enemy != self && !activeAdrenalines.ContainsKey(enemy))
                {
                    Adrenaline(enemy);
                    buffedCount++;
                    if (buffedCount >= 3) break; // До 3-х врагов
                }
            }

            if (buffedCount > 0)
            {
                adrenalineTimer = 0; // Сбрасываем только если хоть кто-то получил бафф
            }
        }

        // 3. Логика Разрушителя сердец
        // Активируется сам, если готов кулдаун и накоплены стаки
        if (attacksCount == 4 && atkComp.cooldown < 0.1f)
        {
            if (atkComp.targetsInRange.Count > 0)
            {
                HeartBreak(atkComp.targetsInRange[0]);
            }
        }
    }


    public void GepAndWarf(EntityData target)
    {
        if (target == null) return;

        if (activeBleeds.ContainsKey(target)) return;

        Coroutine bleedRoutine = StartCoroutine(BleedRoutine(target));
        activeBleeds.Add(target, bleedRoutine);

        Debug.Log($"ГсВ {target}");
    }

    private IEnumerator BleedRoutine(EntityData target)
    {
        float elapsed = 0;

        while (elapsed < bleedDuration)
        {
            // Проверяем, не умер ли юнит от чего-то другого
            if (target == null || target.HP <= 0) break;

            target.TakeDamage(bleedDamage);

            yield return new WaitForSeconds(bleedTickInterval); // Тик раз в секунду
            elapsed += bleedTickInterval;
        }

        // Чистим словарь по окончании
        if (target != null) activeBleeds.Remove(target);
    }


    public void Adrenaline(EntityData ally)
    {
        if (ally == null || ally == self) return;

        if (activeAdrenalines.ContainsKey(ally)) return;

        activeAdrenalines.Add(ally, StartCoroutine(AdrenalineRoutine(ally)));
        Debug.Log($"Адреналинчик {ally}");
    }

    private IEnumerator AdrenalineRoutine(EntityData ally)
    {
        if (ally == null) yield break;

        // Запоминаем старые статы, чтобы потом вернуть
        int bonusAtk = Mathf.RoundToInt(ally.ATK * atkBuffMultiplier);
        float bonusSpeed = (ally.speed * speedBuffMultiplier);

        ally.ATK += bonusAtk;
        ally.speed += bonusSpeed;

        if (core.currentStage == 1)
        {
            // ЛОГИКА 1 СТАДИИ: Просто ждем длительность и выходим
            yield return new WaitForSeconds(buffDuration);
        }
        else
        {
            // ЛОГИКА 2 СТАДИИ: Бесконечный бафф, пока в области + buffDuration после

            // 1. Пока враг в списке EnemiesList — просто ждем (бафф висит)
            while (ally != null && EnemiesList.targetsInRange.Contains(ally))
            {
                yield return new WaitForSeconds(0.5f); // Проверка раз в полсекунды для оптимизации
            }

            // 2. Как только враг вышел из списка (или список пуст) — ждем финальный buffDuration
            yield return new WaitForSeconds(buffDuration);
        }

        // Возвращаем как было, если союзник еще жив
        if (ally != null)
        {
            ally.ATK -= bonusAtk;
            ally.speed -= bonusSpeed;
        }

        activeAdrenalines.Remove(ally);
    }


    public void HeartBreakInc()
    {
        if (attacksCount != 4) attacksCount++;
    }

    public void HeartBreak(EntityData target)
    {
        if (target == null) return;

        if (attacksCount == 4)
        {
            int hbAtk = Mathf.RoundToInt(self.ATK * atkMultiplier);
            target.TakeDamage(Damage.CalculateFromAttack(hbAtk, AtkType.Physical, target));
            atkComp.cooldown = self.attackInterval * 3;
            attacksCount = 0;
        }
        Debug.Log($"Крушитель {target}");
    }

    public void Stage2Transition()
    {
        StopAllCoroutines();
        activeBleeds.Clear();
        activeAdrenalines.Clear();
        attacksCount = 0;

        lastHpCheck = self.maxHP;
    }


    public void LoveRedistribution()
    {
        auraTickTimer += Time.deltaTime;

        if (auraTickTimer >= auraTickInterval) // Тик ауры раз в секунду
        {
            // 1. Воздействие на Юнитов (Урон и стаки Любви)
            // Делаем копию списка, на случай если юнит умрет во время цикла
            List<EntityData> currentUnits = new List<EntityData>(UnitsList.targetsInRange);

            foreach (var unit in currentUnits)
            {
                if (unit == null || unit.HP <= 0) continue;

                // Наносим урон ауры
                unit.TakeDamage(Damage.CalculateFromAttack(auraTickAtk, AtkType.Magical, unit));

                // Нанесенный урон конвертируется в стаки Любви
                loveStacks += auraTickAtk;
                Debug.Log($"Аура дамажит {unit}");
            }

            if (loveStacks >= loveMax) LoveKills();

            auraTickTimer = 0; // Сброс таймера тика
        }
    }


    public void AdrenalineAura()
    {
        foreach (var enemy in EnemiesList.targetsInRange)
        {
            if (enemy == null || enemy == self) continue;

            Adrenaline(enemy);
        }
    }


    public void LoveKills()
    {
        if (loveStacks >= loveMax) loveStacks -= loveMax;

        int explosionAtk = Mathf.RoundToInt(self.ATK * explosionAtkMultiplier);

        List<EntityData> targetsToHit = new List<EntityData>(UnitsList.targetsInRange);
        foreach (var unit in targetsToHit)
        {
            if (unit != null && unit.HP > 0)
            {
                 int finalDmg = Damage.CalculateFromAttack(explosionAtk, AtkType.Magical, unit);
                 unit.TakeDamage(finalDmg);
            }
        }
        Debug.Log("взрыв");
    }
}
