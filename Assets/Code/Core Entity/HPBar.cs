using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public Transform bar;
    public GameObject barBG;

    private EntityData entity;

    public void Init()
    {
        entity = GetComponentInParent<EntityData>();

        entity.OnHPChanged += UpdateBar;

        UpdateBar(entity.HP, entity.maxHP);
    }

    private void UpdateBar(int current, int max)
    {
        float percent = (float)current / max;
        percent = Mathf.Clamp01(percent);

        bar.localScale = new Vector3(percent, 0.07f, 1);

        // �������� ���, ���� ��� ���� � � ���� ������ HP
        if (entity.entityType == EntityType.Enemy && current == max)
        {
            bar.gameObject.SetActive(false);
            barBG.SetActive(false);
        }
        else
        {
            bar.gameObject.SetActive(true);
            barBG.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (entity != null)
            entity.OnHPChanged -= UpdateBar;
    }
}

//public class Bars : MonoBehaviour
//{
//    public int maxHP; // ������������ HP, ����� ��� ������� �������� HP
//    public int currentHP; //������� HP, ������� ����� ����������� � Update() ��� ����������� �� HP ����
//    public EntityData me; // � - ������ �� EntityData ��� ��������� �������� HP
//    [Space]
//    [Header("HP Bar")]
//    public GameObject barBG; // ������� ����� HP ����, ������������ ������������ HP � �������� ����� ��� ����������� �����
//    public GameObject barFill; // ����������� ����� HP ����, ������������ ������� HP
//    public Transform bar; // ������ �� Transform ����������� ����� HP ����, ��� ��������� ��� �������� � ����������� �� �������� HP

//    void Start() //��� ������, �������� ������ �� EntityData � ������������ �������, ������������� maxHP �� EntityData (��� ������: �������� HP ���, ���� HP �����)
//    {
//        me = GetComponentInParent<EntityData>();
//        maxHP = me.maxHP;
//        if (transform.parent.CompareTag("Enemy"))
//        {
//            barBG.SetActive(false); // �������� ��� ������ HP
//            barFill.SetActive(false);
//        }
//    }

//    void Update() //������ ����, ��������� currentHP �� EntityData, ���������� HP ���, ���� HP �� �����, ����������� ������� HP � �������� ������� ����������� ����� � ����������� �� �������� HP
//    {
//        currentHP = me.HP;
//        while (currentHP != maxHP)
//        {
//            barBG.SetActive(true);
//            barFill.SetActive(true);
//            break;
//        }
//        float HPpercent = (float)currentHP / maxHP;
//        bar.localScale = new Vector3(HPpercent, 0.07f, 1);
//    }
//}