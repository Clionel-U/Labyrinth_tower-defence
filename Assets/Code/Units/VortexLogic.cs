using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexLogic : MonoBehaviour
{
    public float rotationSpeed; // Скорость вращения
    public int vortexAtk;                 // Передается из скилла
    public float damageInterval; // Как часто наносить урон одному врагу

    private float timer;

    public GameObject inner;
    public GameObject outer;

    // Словарь для контроля кулдауна урона по каждому врагу
    private Dictionary<EntityData, float> damageTimers = new Dictionary<EntityData, float>();

    void Update()
    {
        // Вращаем весь объект с душами
        inner.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        outer.transform.Rotate(0, 0, 1.5f * -rotationSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= damageInterval - 0.1)
        {
            CleanupTimers();
            timer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EntityData enemy = other.GetComponent<EntityData>();
        if (enemy == null || enemy.entityType != EntityType.Enemy) return;
        if (enemy.enemyType == EnemyType.Air) return;

        if (!damageTimers.ContainsKey(enemy))
        {
            damageTimers.Add(enemy, 0);
        }

        if (Time.time >= damageTimers[enemy])
        {
            // Наносим урон
            int damage = Damage.CalculateFromAttack(vortexAtk, AtkType.Magical, enemy);
            enemy.TakeDamage(damage);
            // Ставим кулдаун для этого конкретного врага
            damageTimers[enemy] = Time.time + damageInterval;
        }
    }

    private readonly List<EntityData> toRemove = new List<EntityData>();

    private void CleanupTimers()
    {
        toRemove.Clear();

        // Собираем всех, кто больше не валиден
        foreach (var enemy in damageTimers)
        {
            // Если враг умер (null) или отошел слишком далеко от вихря
            // (чтобы не хранить таймеры тех, кто уже на другом конце карты)
            if (enemy.Key == null)
            {
                toRemove.Add(enemy.Key);
            }
        }

        // Безопасно удаляем
        foreach (var entity in toRemove)
        {
            damageTimers.Remove(entity);
        }
    }
}
