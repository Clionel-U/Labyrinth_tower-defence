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
    public float cooldown = 0f;
    //private bool readyAttack = true;

    public System.Action onAttack;

    private void Awake()
    {
        self = GetComponent<EntityData>();
    }

    private void Update()
    {
        if (cooldown > 0f) // ��������� ������� ���� �� ����
            cooldown -= Time.deltaTime;

        targetsInRange.RemoveAll(t => t == null); // ������ ������

        if (targetsInRange.Count == 0) return; // ���� ��� ������ � ������ �� ������
                
        if (cooldown > 0f) return; // ���� �� �� ������ � �� �������

        EntityData target = targetsInRange[0]; // �������� ����

        if (target.isInvulnerable)
        {
            if (targetsInRange.Count > 1)
                target = targetsInRange[1]; // ���� ������ ���� ���������, ������� ������
            else
                return; // ���� ������ ���� ��� � ������ �� ������
        }

        if (doubleAttack)
        {
            // ������ ��� � ����������
            AtkType originalType = self.atkType;

            self.atkType = AtkType.Physical;
            int physDmg = Damage.Calculate(self, target);
            target.TakeDamage(physDmg);

            // ������ ��� � ���������� (������ ���� ���� ��� ����)
            if (target != null)
            {
                self.atkType = AtkType.Magical;
                int magDmg = Damage.Calculate(self, target);
                target.TakeDamage(magDmg);
            }
                        
            self.atkType = originalType; // ��������������� ������������ ��� �����
        }
        else
        {
            int dmg = Damage.Calculate(self, target);
            target.TakeDamage(dmg);
        }

        cooldown = self.attackInterval; // ��������� ������� ����� �����

        Skill skill = GetComponent<Skill>();
        if (skill != null && skill.spChargeType == SPChargeType.PerAttack)
            skill.ChargePerAttack();

        onAttack?.Invoke();
    }
}