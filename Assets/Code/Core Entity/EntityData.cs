using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType { Unit, Enemy } //тип сущности, юнит или враг, влияет на то, какие атрибуты у неё есть и как она взаимодействует с другими объектами
public enum UnitType { Melee, Ranged } //куда ставить юнита, на землю или на возвышенность
public enum EnemyType { Ground, Air } //тип врага, наземный или воздушный, влияет на то, может ли юнит его атаковать
public enum AtkType { Physical, Magical } //тип атаки, влияет на то, как рассчитывается урон (ATK-DEF или ATK-RES)

public class EntityData : MonoBehaviour
{
    [Header("Entity Attributes")] //общие атрибуты для юнитов и врагов
    public string _name; //название
    public Sprite icon; //иконка, для UI
   public EntityType entityType; //тип сущности, юнит или враг, влияет на то, какие атрибуты у неё есть и как она взаимодействует с другими объектами
    public int HP; //текущее здоровье, может изменяться в бою
    public int maxHP; //максимальное базовое здоровье, нужно для восстановления здоровья и отображения HP бара
    public int ATK; //текущая атака, может изменяться баффами и дебаффами
    public int baseATK; //базовая атака, нужна для расчёта баффов от скиллов
    public int DEF; //текущая защита, может изменяться баффами и дебаффами
    public int RES; //текущая маг. защита, может изменяться баффами и дебаффами
    public float attackInterval; //интервал между атаками, влияет на скорость атаки
    public AtkType atkType;
    public UnitType unitType;
    public int cost; //стоимость юнита, влияет на то, сколько "бития" нужно для его призыва
    [Space]
    [Header("Unit Attributes")] //атрибуты, специфичные для юнитов
    public int maxBlock; //макс. блок, влияет на то, сколько врагов может заблокировать юнит одновременно
    [Space]
    [Header("Enemy Attributes")] //атрибуты, специфичные для врагов
    public int blockNeed; //требуемый блок для блокировки врага, влияет на то, может ли юнит его заблокировать
    public float speed; //скорость врага, влияет на его движение по пути и на анимацию

    public System.Action<int, int> OnHPChanged; // current, max, для обновления HP бара и других UI элементов, зависящих от здоровья
    public System.Action OnDeath; // для обработки смерти юнита или врага, например, для удаления из списка врагов в радиусе атаки

    private void OnEnable()
    {
        HP = maxHP;
        OnHPChanged?.Invoke(HP, maxHP);
        ATK = baseATK;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        HP = Mathf.Max(HP, 0);

        OnHPChanged?.Invoke(HP, maxHP);

        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
