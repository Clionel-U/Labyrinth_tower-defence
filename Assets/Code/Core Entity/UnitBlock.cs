using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitBlock : MonoBehaviour
{
    private EntityData self;
    private AttackTargetInRange targetList;
    [SerializeField] private int block;

    private void Awake()
    {
        self = GetComponentInParent<EntityData>();
        targetList = GetComponentInParent<AttackTargetInRange>();
        block = self.maxBlock;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBlock enemyBlock = other.GetComponent<EnemyBlock>();
        if (enemyBlock == null) return;

        EntityData enemy = enemyBlock.self;
        if (enemy == null || enemy.entityType != EntityType.Enemy) return;

        TryBlock(enemy, enemyBlock);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyBlock enemyBlock = other.GetComponent<EnemyBlock>();
        if (enemyBlock == null) return;

        EntityData enemy = enemyBlock.self;
        if (enemy == null || enemy.entityType != EntityType.Enemy) return;

        Unblock(enemy, enemyBlock);
    }

    public bool TryBlock(EntityData enemy, EnemyBlock enemyBlock)
    {
        if (enemyBlock == null || enemy == null) return false;

        // уже заблокирован
        if (enemyBlock.IsBlocked()) return false;

        // лимит блока
        if (block <= 0 || block < enemy.blockNeed) return false;

        //блок
        enemyBlock.SetBlocker(self, this);
        block -= enemy.blockNeed;
        
        // добавляем в начало списка целей юнита
        targetList.targetsInRange.Remove(enemy);
        targetList.targetsInRange.Insert(0, enemy);       

        return true;
    }

    public void Unblock(EntityData enemy, EnemyBlock enemyBlock)
    {
        if (enemyBlock == null || enemy == null) return;

        if (enemyBlock.blocker == self)
        {
            block += enemy.blockNeed;
            if (block > self.maxBlock) block = self.maxBlock;
            enemyBlock.ClearBlock();
        }
    }
}