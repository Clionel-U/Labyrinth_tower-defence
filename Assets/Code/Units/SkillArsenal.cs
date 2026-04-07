using UnityEngine;

public class SkillArsenal : Skill
{
    [Header("Арсенал двойников")]
    public float atkIncreasePercent = 0.3f;  // бафф атаки во время скилла
    public Sprite activeSprite;          // спрайт во время скилла
    public Sprite normalSprite;          // исходный спрайт

    private SpriteRenderer sr;
    private int buff;
    private AttackTargetInRange attackComp;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = true;
        maxSP = 10f;
        duration = 8f;

        sr = GetComponent<SpriteRenderer>();
        attackComp = GetComponent<AttackTargetInRange>();
    }

    protected override void OnSkillActivate()
    {
        buff = Mathf.RoundToInt(self.baseATK * atkIncreasePercent);

        self.ATK += buff;

        if (activeSprite != null)
            sr.sprite = activeSprite;

        // Включаем двойную атаку
        attackComp.doubleAttack = true;
    }

    protected override void OnSkillDeactivate()
    {
        self.ATK -= buff;

        if (normalSprite != null)
            sr.sprite = normalSprite;

        attackComp.doubleAttack = false;
    }
}