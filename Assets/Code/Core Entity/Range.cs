using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    public AttackTargetInRange targetList; //������ �� ������, ������� ������ ������ ������ � ������� ����� � ��������� �����
    private EntityType targets; //���, ������� ����������, ���� ������� ������� (��� ������ ��� "Enemy", ��� ������ ��� "Unit")
    private EntityData self;

    // ����� ���� ������ ����� ��������� ���� ����
    public bool canAttackGround = true;
    public bool canAttackAir;

    private void OnEnable() //��� ��������� �������, ������� ������ ������ � ������� � ������������� ��� ��� ����������� ������
    {
        targetList.targetsInRange.Clear();
        self = GetComponentInParent<EntityData>();

        if (self.entityType == EntityType.Unit)
            targets = EntityType.Enemy;
        else if (self.entityType == EntityType.Enemy)
            targets = EntityType.Unit;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityData entity = other.GetComponent<EntityData>();
        if (entity == null || entity.entityType != targets) return;
        if (!CanTarget(entity)) return;

        if (!targetList.targetsInRange.Contains(entity))
        {
            targetList.targetsInRange.Add(entity);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        EntityData entity = other.GetComponent<EntityData>();
        if (entity == null || entity.entityType != targets) return;
        
        targetList.targetsInRange.Remove(entity);
    }

    public bool CanTarget(EntityData entity)
    {
        if (entity.entityType == EntityType.Enemy)
        {
            if (entity.enemyType == EnemyType.Ground && !canAttackGround) return false;
            if (entity.enemyType == EnemyType.Air && !canAttackAir) return false;
        }
        return true;
    }
}

//private void OnTriggerEnter2D(Collider2D other) //����� ������ ������ ������ � ������, ��������� ��� ���. ���� �� ������������� ����, �� �������� �������� ��� �������� �, ���� ����� ��� ��� � ������, �������� � ������ ������ � �������
//{
//    if (other.CompareTag(_tag))
//    {
//        EntityData enemy = other.GetComponent<EntityData>();
//        if (enemy != null && !targetList.targetsInRange.Contains(enemy))
//        {
//            targetList.targetsInRange.Add(enemy);
//        }
//    }
//}

//private void OnTriggerExit2D(Collider2D other) //����� ������ ������ ������� �� �������, ��������� ��� ���. ���� �� ������������� ����, �� �������� �������� ��� �������� �, ���� ���� ���� � ������, ������� �� ������ ������ � �������
//{
//    if (other.CompareTag(_tag))
//    {
//        EntityData enemy = other.GetComponent<EntityData>();
//        if (enemy != null)
//        {
//            targetList.targetsInRange.Remove(enemy);
//        }
//    }
//}