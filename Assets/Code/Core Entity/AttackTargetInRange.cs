using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackTargetInRange : MonoBehaviour
{
    public List<EntityData> targetsInRange = new List<EntityData>();
    public bool doubleAttack = false;

    private EntityData self;
    private float cooldown = 0f;
    //private bool readyAttack = true;

    private void Awake()
    {
        self = GetComponent<EntityData>();
    }

    private void Update()
    {
        if (cooldown > 0f) // уменьшаем кулдаун если он есть
            cooldown -= Time.deltaTime;

        targetsInRange.RemoveAll(t => t == null); // чистим список

        if (targetsInRange.Count == 0) return; // если нет врагов Ч ничего не делаем
                
        if (cooldown > 0f) return; // если кд не прошЄл Ч не атакуем

        EntityData target = targetsInRange[0]; // выбираем цель

        if (doubleAttack)
        {
            // ѕервый хит Ч физический
            AtkType originalType = self.atkType;

            self.atkType = AtkType.Physical;
            int physDmg = Damage.Calculate(self, target);
            target.TakeDamage(physDmg);

            // ¬торой хит Ч магический (только если цель ещЄ жива)
            if (target != null)
            {
                self.atkType = AtkType.Magical;
                int magDmg = Damage.Calculate(self, target);
                target.TakeDamage(magDmg);
            }
                        
            self.atkType = originalType; // ¬осстанавливаем оригинальный тип атаки
        }
        else
        {
            int dmg = Damage.Calculate(self, target);
            target.TakeDamage(dmg);
        }

        cooldown = self.attackInterval; // запускаем кулдаун после атаки

        Skill skill = GetComponent<Skill>();
        if (skill != null && skill.spChargeType == SPChargeType.PerAttack)
            skill.ChargePerAttack();
    }
}