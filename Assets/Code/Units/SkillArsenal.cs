using UnityEngine;

public class SkillArsenal : Skill
{
    [Header("������� ���������")]
    public float atkIncreasePercent = 0.3f;  // ���� ����� �� ����� ������
    public Sprite activeSprite;          // ������ �� ����� ������
    public Sprite normalSprite;          // �������� ������

    private SpriteRenderer sr;
    private int buff;
    private AttackTargetInRange attackComp;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = true;
        maxSP = 15f;
        duration = 10f;

        sr = GetComponent<SpriteRenderer>();
        attackComp = GetComponent<AttackTargetInRange>();
    }

    protected override void OnSkillActivate()
    {
        buff = Mathf.RoundToInt(self.baseATK * atkIncreasePercent);

        self.ATK += buff;

        if (activeSprite != null)
            sr.sprite = activeSprite;

        // �������� ������� �����
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