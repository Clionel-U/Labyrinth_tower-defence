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

    public static int CalculateFromAttack(int damageFrom, AtkType _atkType, EntityData target)
    {
        int damage = 0;
        if (_atkType == AtkType.Physical)
            damage = damageFrom - target.DEF;

        else if (_atkType == AtkType.Magical)
            damage = damageFrom * (100 - target.RES) / 100;

        return Mathf.Max(damage, 0);

    }
}