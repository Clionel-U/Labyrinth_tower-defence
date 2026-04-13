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
    public string _name;
    public Sprite icon;
    public EntityType entityType;
    public int HP;
    public int maxHP;
    public int ATK;
    public int baseATK;
    public int DEF;
    public int RES;
    public float attackInterval;
    public int cost;
    [Space]
    public AtkType atkType;

    public GameObject HPBarPrefab;
    private GameObject healthBar;
    public GameObject SPBarPrefab;
    private GameObject skillBar;

    [Header("Unit Attributes")] //атрибуты, специфичные для юнитов
    public int maxBlock;
    public int redeployTime;

    public UnitType unitType;


    [Header("Unit Technical")]
    public int unitID;
    public Vector3 occupiedCell;
    public float healBonus = 0;

    [Header("Enemy Attributes")] //атрибуты, специфичные для врагов
    public int blockNeed;
    public float speed;
    public EnemyType enemyType;

    [Header("Boss Settings")]
    public bool isBoss;
    public bool isInvulnerable = false;

    public System.Action<int, int> OnHPChanged;
    public System.Action OnDeath;

    private void OnEnable()
    {
        if (entityType == EntityType.Enemy) AllEnemies.allEnemies.Add(this);

        //HP bar
        Vector3 pos = transform.position;
        pos.x -= 0.45f;
        pos.y -= 0.3498f;
        healthBar = Instantiate(HPBarPrefab, pos, Quaternion.identity);
        healthBar.transform.SetParent(transform);
        healthBar.GetComponent<HPBar>().Init();

        //SP bar
        if (entityType == EntityType.Unit)
        {
            pos = transform.position;
            pos.x -= 0.45f;
            pos.y -= 0.42f;
            skillBar = Instantiate(SPBarPrefab, pos, Quaternion.identity);
            skillBar.transform.SetParent(transform);
            Skill skill = GetComponent<Skill>();
            if (skill != null)
                skillBar.GetComponent<SPBar>().Init(skill);
        }

        HP = maxHP;
        OnHPChanged?.Invoke(HP, maxHP);
        ATK = baseATK;
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        HP -= damage;
        HP = Mathf.Max(HP, 0);

        OnHPChanged?.Invoke(HP, maxHP);

        if (HP <= 0)
        {
            if (isBoss) GetComponent<BossCore>().HandleDeath();
            else Die();
        }
        else StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    public void Heal(int amount)
    {
        int heal = Mathf.RoundToInt(amount * (1 + healBonus));
        HP = Mathf.Min(HP + heal, maxHP);
        OnHPChanged?.Invoke(HP, maxHP);
    }

    public void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnHPChanged = null;
        OnDeath = null;

        if (entityType == EntityType.Unit)
        {
            GridManager.Instance?.occupiedPositions.Remove(occupiedCell);
            BitiumSystem.Instance?.BitiumChange();
        }
        
        if (entityType == EntityType.Enemy) AllEnemies.allEnemies.Remove(this);
    }
}
