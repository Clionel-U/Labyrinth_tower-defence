using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTargetInRange : MonoBehaviour
{
    public List<EntityData> targetsInRange = new List<EntityData>();

    public bool healAll;

    private EntityData self;
    public float cooldown = 0f;

    private void Awake()
    {
        self = GetComponent<EntityData>();
    }

    private void Update()
    {
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        targetsInRange.RemoveAll(t => t == null);

        if (targetsInRange.Count == 0) return;

        if (cooldown > 0f) return;

        if (!healAll)
        {
            // Ищем союзника с наименьшим HP
            EntityData target = null;
            float lowestHPPercent = float.MaxValue;

            foreach (var ally in targetsInRange)
            {
                if (ally.HP >= ally.maxHP) continue;

                float percent = (float)ally.HP / ally.maxHP;
                if (percent < lowestHPPercent)
                {
                    lowestHPPercent = percent;
                    target = ally;
                }
            }

            if (target == null) return; // все здоровы

            target.Heal(self.ATK);
        }

        if (healAll)
        {
            foreach (var ally in targetsInRange)
            {
                if (ally.HP >= ally.maxHP) continue;
                ally.Heal(self.ATK);
            }
        }

        cooldown = self.attackInterval;

        // Зарядка SP за атаку
        Skill skill = GetComponent<Skill>();
        if (skill != null && skill.spChargeType == SPChargeType.PerAttack)
            skill.ChargePerAttack();

        
    }
}
