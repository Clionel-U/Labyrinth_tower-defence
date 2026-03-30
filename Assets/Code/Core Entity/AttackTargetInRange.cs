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

        // если нет врагов — ничего не делаем
        if (targetsInRange.Count == 0) return;

        // если кд не прошёл — не атакуем
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

//private void Start()
//{
//    StartCoroutine(AttackLoop());
//}

//IEnumerator AttackLoop()
//{
//    while (readyAttack)
//    {
//        if (targetsInRange.Count != 0)
//        {
//            targetsInRange.RemoveAll(t => t == null); // очищаем мёртвых
//            if (targetsInRange.Count != 0)
//            {
//                EntityData target = targetsInRange[0];

//                int dmg = Damage.Calculate(self, target);
//                target.TakeDamage(dmg);
//                readyAttack = false;
//            }
//        }

//    }
//    while (!readyAttack)
//    {
//        yield return new WaitForSeconds(self.attackInterval);
//        readyAttack = true;
//    }

//}

//while (true)
//{
//    yield return new WaitForSeconds(self.attackInterval);

//    if (targetsInRange.Count == 0) continue;

//    // очищаем мёртвых
//    targetsInRange.RemoveAll(t => t == null);

//    if (targetsInRange.Count == 0) continue;

//    EntityData target = targetsInRange[0];

//    int dmg = Damage.Calculate(self, target);
//    target.TakeDamage(dmg);
//}

//public class AttackTargetInRange : MonoBehaviour
//{
//    public List<EntityData> targetsInRange = new List<EntityData>(); //список врагов в радиусе атаки
//    private EntityData attributes; //атрибуты атакующего существа(юнит или враг), для удобства доступа к ним
//    public Damage damage; //ссылка на скрипт, который рассчитывает урон и применяет его к врагу
//    public bool ready = true; //флаг, который показывает, готов ли юнит атаковать (учитывает интервал между атаками)

//    private void OnEnable() //при активации объекта, получаем ссылку на атрибуты существа, к которому прикреплен этот скрипт (для юнита это будут его атрибуты, для врага - его атрибуты)
//    {
//        attributes = GetComponent<EntityData>();
//    }

//    void Update()
//    {
//        if (ready == true && targetsInRange.Count > 0) //если юнит готов атаковать и в радиусе есть враги
//        {
//            EntityData target = targetsInRange[0]; //выбираем первого врага в списке (можно изменить логику выбора, например, на ближайшего врага)
//            //  Enemy target = attackRadius.enemiesInRange[attackRadius.enemiesInRange.Count - 1]; //выбираем последнего врага в списке (можно изменить логику выбора, например, на ближайшего врага) (не используется)
//            StartCoroutine(AttackEnemy(target)); //запускаем атаку (корутину)
//            ready = false; //идем на перезарядку
//        }
//    }

//    IEnumerator AttackEnemy(EntityData target) //корутина, которая выполняет атаку и ждет интервал между атаками
//    {
//        damage.DealDamage(attributes, target); //посылаем атрибуты атакующего и цели в скрипт, который рассчитывает урон и применяет его к врагу
//        Debug.Log(gameObject + "атакует" + target); //для отладки, выводим в консоль, кто атакует кого (можно удалить позже)
//        yield return new WaitForSeconds(attributes.attackInterval); //ждем интервал между атаками, который задается в атрибутах существа
//        ready = true; //после ожидания, юнит снова готов атаковать
//    }
//}