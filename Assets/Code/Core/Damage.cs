using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Damage
{
    public static int Calculate(EntityData attacker, EntityData target)
    {
        int damage = 0;

        if (attacker.atkType == AtkType.Physical)
            damage = attacker.ATK - target.DEF;

        else if (attacker.atkType == AtkType.Magical)
            damage = attacker.ATK * (100 - target.RES) / 100;

        return Mathf.Max(damage, 0);
    }
}

//public class Damage : MonoBehaviour
//{   
//    public int finalDamage; // итоговый урон, который будет нанесён цели после всех расчётов

//    public void DealDamage(EntityData attacker, EntityData target) //метод, который принимает атрибуты атакующего и цели, рассчитывает урон в зависимости от типа атаки атакующего и применяет его к цели
//    {   
//        if (attacker.atkType == AtkType.Physical)
//        {
//            finalDamage = attacker.ATK - target.DEF;            
//        }
//        if (attacker.atkType == AtkType.Magical)
//        {
//            finalDamage = attacker.ATK * (100-target.RES) / 100;
//        }
//        finalDamage = Mathf.Max(finalDamage, 0);
//        target.HP -= finalDamage;
//    }
//}