using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBlock : MonoBehaviour
{
    public EntityData blocker; // кто блокирует врага

    private EntityData self;
    private AttackTargetInRange targetList;

    private void Awake()
    {
        self = GetComponent<EntityData>();
        targetList = GetComponent<AttackTargetInRange>();
    }

    public bool IsBlocked()
    {
        return blocker != null;
    }

    public void SetBlocker(EntityData unit)
    {
        blocker = unit;
        // добавляем юнита в начало целей врага
        if (targetList != null)
        {
            targetList.targetsInRange.Remove(unit);
            targetList.targetsInRange.Insert(0, unit);
        }
    }

    public void ClearBlock()
    {
        if (targetList != null && blocker != null)
        {
            targetList.targetsInRange.Remove(blocker);
        }

        blocker = null;
    }

    private void OnDestroy()
    {
        if (blocker != null)
        {
            UnitBlock unitBlock = blocker.GetComponent<UnitBlock>();
            if (unitBlock != null)
            {
                unitBlock.Unblock(self);
            }
        }
    }
}

//public class EnemyBlock : MonoBehaviour
//{
//    public bool blocked = false;
//    public Block blockingUnit;

//    public void Blocked(Block unit)
//    {
//        GetComponent<Move>().enabled = false;
//        blockingUnit = unit;
//    }

//    private void OnDestroy()
//    {
//        blockingUnit.block += GetComponent<EntityData>().blockNeed;
//    }
//}
