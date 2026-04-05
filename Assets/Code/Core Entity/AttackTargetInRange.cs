using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackTargetInRange : MonoBehaviour
{
    public List<EntityData> targetsInRange = new List<EntityData>();

    private EntityData self;
    private float cooldown = 0f;
    //private bool readyAttack = true;

    private void Awake()
    {
        self = GetComponent<EntityData>();
    }

    private void Update()
    {
        // уменьшаем кулдаун если он есть
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        // чистим список
        targetsInRange.RemoveAll(t => t == null);

        // если нет врагов Ч ничего не делаем
        if (targetsInRange.Count == 0) return;

        // если кд не прошЄл Ч не атакуем
        if (cooldown > 0f) return;

        // выбираем цель
        EntityData target = targetsInRange[0];

        // атакуем
        int dmg = Damage.Calculate(self, target);
        target.TakeDamage(dmg);

        // запускаем кулдаун после атаки
        cooldown = self.attackInterval;
    }
}