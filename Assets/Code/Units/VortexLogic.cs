using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexLogic : MonoBehaviour
{
    public float rotationSpeed; // �������� ��������
    public int vortexAtk;                 // ���������� �� ������
    public float damageInterval; // ��� ����� �������� ���� ������ �����

    private float timer;

    public GameObject inner;
    public GameObject outer;

    // ������� ��� �������� �������� ����� �� ������� �����
    private Dictionary<EntityData, float> damageTimers = new Dictionary<EntityData, float>();

    void Update()
    {
        // ������� ���� ������ � ������
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
            // ������� ����
            int damage = Damage.CalculateFromAttack(vortexAtk, AtkType.Magical, enemy);
            enemy.TakeDamage(damage);
            // ������ ������� ��� ����� ����������� �����
            damageTimers[enemy] = Time.time + damageInterval;
        }
    }

    private readonly List<EntityData> toRemove = new List<EntityData>();

    private void CleanupTimers()
    {
        toRemove.Clear();

        // �������� ����, ��� ������ �� �������
        foreach (var enemy in damageTimers)
        {
            // ���� ���� ���� (null) ��� ������ ������� ������ �� �����
            // (����� �� ������� ������� ���, ��� ��� �� ������ ����� �����)
            if (enemy.Key == null)
            {
                toRemove.Add(enemy.Key);
            }
        }

        // ��������� �������
        foreach (var entity in toRemove)
        {
            damageTimers.Remove(entity);
        }
    }
}
