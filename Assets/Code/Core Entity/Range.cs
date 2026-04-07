using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    public AttackTargetInRange targetList; //ссылка на скрипт, который хранит список врагов в радиусе атаки и выполняет атаку
    public EntityType targets; //тег, который определяет, кого считать врагами (для юнитов это "Enemy", для врагов это "Unit")
    private EntityData self;

    // Какие типы врагов может атаковать этот юнит
    public bool canAttackGround = true;
    public bool canAttackAir;

    private void OnEnable() //при активации объекта, очищаем список врагов в радиусе и устанавливаем тег для определения врагов
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

//private void OnTriggerEnter2D(Collider2D other) //когда другой объект входит в радиус, проверяем его тег. если он соответствует тегу, то пытаемся получить его атрибуты и, если врага еще нет в списке, добавить в список врагов в радиусе
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

//private void OnTriggerExit2D(Collider2D other) //когда другой объект выходит из радиуса, проверяем его тег. если он соответствует тегу, то пытаемся получить его атрибуты и, если враг есть в списке, удалить из списка врагов в радиусе
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