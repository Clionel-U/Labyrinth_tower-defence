using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SPChargeType
{
    PerSecond,  // каждую секунду (Арсенал, Раздор, Нейросимфония и др.)
    PerAttack,  // каждую атаку юнита (Метаморфоза, Препарат NT-X7, Магические пули)
    None        // SP нет (Мученица — активируется при деплое)
}

public enum ActivationType
{
    Manual,     // ручная активация игроком
    Auto,       // автоматически когда SP заполнен
    OnDeploy    // один раз при деплое
}

public abstract class Skill : MonoBehaviour
{
    [Header("Настройки SP")]
    public SPChargeType spChargeType;
    public ActivationType activationType;
    public float maxSP = 10f;
    public float currentSP = 0f;

    [Header("Длительность")]
    public bool hasDuration;        // есть ли ограниченная длительность
    public float duration;          // секунды (0 = бесконечно пока не выключат)
    public bool canToggleOff;       // можно ли выключить вручную (Метаморфоза)

    [Header("Состояние")]
    public bool isActive = false;
    public float durationTimer = 0f;

    protected EntityData self;
    protected AttackTargetInRange targetList;

    public System.Action OnSPChanged;   // для UI полоски SP
    public System.Action OnActivated;
    public System.Action OnDeactivated;

    protected virtual void Awake()
    {
        self = GetComponent<EntityData>();
        targetList = GetComponent<AttackTargetInRange>();
    }

    protected virtual void Start()
    {
        // Мученица — активируется сразу при деплое
        if (activationType == ActivationType.OnDeploy)
            Activate();
    }

    protected virtual void Update()
    {
        if (isActive && hasDuration)
        {
            durationTimer -= Time.deltaTime;
            if (durationTimer <= 0f)
                Deactivate();
        }

        // Зарядка SP по времени — прямо здесь, не в дочерних классах
        if (!isActive && spChargeType == SPChargeType.PerSecond)
            ChargePerSecond();

        // Авто-активация
        if (!isActive && activationType == ActivationType.Auto)
        {
            if (currentSP >= maxSP)
                Activate();
        }
    }

    //  Зарядка SP 

    // Вызывается из Update скилла (для PerSecond)
    protected void ChargePerSecond()
    {
        if (isActive) return; // во время скилла SP не копится
        AddSP(Time.deltaTime);
    }

    // Вызывается из AttackTargetInRange когда юнит атакует (для PerAttack)
    public void ChargePerAttack(float amount = 1f)
    {
        if (isActive) return;
        AddSP(amount);
    }

    public void AddSP(float amount)
    {
        if (isActive) return;
        currentSP = Mathf.Min(currentSP + amount, maxSP);
        OnSPChanged?.Invoke();

        if (activationType == ActivationType.Auto && currentSP >= maxSP)
            Activate();
    }

    //  Активация / деактивация 

    public void Activate()
    {
        // Toggle-скиллы (Метаморфоза)
        if (isActive && canToggleOff)
        {
            Deactivate();
            return;
        }

        if (isActive) return;
        
        // Ручная активация требует полного SP
        if (activationType == ActivationType.Manual && currentSP < maxSP) return;
        

        isActive = true;

        if (hasDuration)
            durationTimer = duration;

        if (activationType != ActivationType.OnDeploy)
            currentSP = 0f;

        OnSPChanged?.Invoke();
        OnActivated?.Invoke();
        OnSkillActivate();
    }

    public void Deactivate()
    {
        isActive = false;
        durationTimer = 0f;
        OnDeactivated?.Invoke();
        OnSkillDeactivate();
    }

    //  Переопределяются в каждом скилле 

    protected abstract void OnSkillActivate();
    protected virtual void OnSkillDeactivate() { }
}