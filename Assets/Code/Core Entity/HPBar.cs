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
        float percent = max > 0 ? (float)current / max : 0f;
        percent = Mathf.Clamp01(percent);

        bar.localScale = new Vector3(percent, 0.07f, 1);

        // скрываем бар, если это враг и у него полное HP
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
//    public int maxHP; // максимальный HP, нужно для расчёта процента HP
//    public int currentHP; //текущий HP, который будет обновляться в Update() для отображения на HP баре
//    public EntityData me; // я - ссылка на EntityData для получения текущего HP
//    [Space]
//    [Header("HP Bar")]
//    public GameObject barBG; // фоновая часть HP бара, отображающая максимальный HP и служащая фоном для заполненной части
//    public GameObject barFill; // заполненная часть HP бара, отображающая текущий HP
//    public Transform bar; // ссылка на Transform заполненной части HP бара, для изменения его масштаба в зависимости от процента HP

//    void Start() //при старте, получаем ссылку на EntityData в родительском объекте, устанавливаем maxHP из EntityData (для врагов: скрываем HP бар, если HP полон)
//    {
//        me = GetComponentInParent<EntityData>();
//        maxHP = me.maxHP;
//        if (transform.parent.CompareTag("Enemy"))
//        {
//            barBG.SetActive(false); // скрываем при полном HP
//            barFill.SetActive(false);
//        }
//    }

//    void Update() //каждый кадр, обновляем currentHP из EntityData, отображаем HP бар, если HP не полон, расчитываем процент HP и изменяем масштаб заполненной части в зависимости от процента HP
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