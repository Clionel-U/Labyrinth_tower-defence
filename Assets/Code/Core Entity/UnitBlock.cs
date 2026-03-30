using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitBlock : MonoBehaviour
{
    private EntityData self;
    private AttackTargetInRange targetList;
    [SerializeField]private int block;

    private void Awake()
    {
        self = GetComponent<EntityData>();
        targetList = GetComponent<AttackTargetInRange>();
        block = self.maxBlock;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityData enemy = other.GetComponent<EntityData>();
        if (enemy == null) return;

        if (enemy.entityType != EntityType.Enemy) return;

        TryBlock(enemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EntityData enemy = other.GetComponent<EntityData>();
        if (enemy == null) return;

        if (enemy.entityType != EntityType.Enemy) return;

        Unblock(enemy);
    }

    public bool TryBlock(EntityData enemy)
    {
        if (self.entityType != EntityType.Unit) return false;

        EnemyBlock enemyBlock = enemy.GetComponent<EnemyBlock>();
        
        if (enemyBlock == null) return false;

        // уже заблокирован
        if (enemyBlock.IsBlocked()) return false;

        // лимит блока
        if (block <= 0 || block < enemy.blockNeed) return false;

        //блок
        enemyBlock.SetBlocker(self);
        block -= enemy.blockNeed;
        
        // добавляем в начало списка целей юнита
        targetList.targetsInRange.Remove(enemy);
        targetList.targetsInRange.Insert(0, enemy);       

        return true;
    }

    public void Unblock(EntityData enemy)
    {
        EnemyBlock enemyBlock = enemy.GetComponent<EnemyBlock>();
        if (enemyBlock == null) return;

        if (enemyBlock.blocker == self)
        {
            block += enemy.blockNeed;
            if (block > self.maxBlock) block = self.maxBlock;
            enemyBlock.ClearBlock();
        }
    }
}