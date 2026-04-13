using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFever : Skill
{
    [Header("Обострение")]
    public float attackIntervalMultiplier = 0.5f; // во сколько раз уменьшается интервал

    public Sprite activeSprite;
    public Sprite normalSprite;

    public GameObject normalRange;
    public GameObject skillRange;

    private SpriteRenderer sr;
    private Range skillRangeComp;
    private BoxCollider2D skillRangeCollider;
    private Range normalRangeComp;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Auto;
        hasDuration = true;
        maxSP = 20f;
        duration = 15f;

        sr = GetComponent<SpriteRenderer>();
        normalRangeComp = normalRange.GetComponent<Range>();
        skillRangeComp = skillRange.GetComponent<Range>();
        skillRangeCollider = skillRange.GetComponent<BoxCollider2D>();

        skillRange.SetActive(false);
    }

    protected override void OnSkillActivate()
    {
        // Уменьшаем интервал атаки
        self.attackInterval *= attackIntervalMultiplier;

        // Меняем спрайт
        if (activeSprite != null)
            sr.sprite = activeSprite;

        // Меняем зону
        normalRangeComp.targetList.targetsInRange.Clear();
        normalRange.SetActive(false);
        skillRange.SetActive(true);
        RefreshTargetList();
    }

    protected override void OnSkillDeactivate()
    {
        // Возвращаем интервал атаки
        self.attackInterval /= attackIntervalMultiplier;

        // Возвращаем спрайт
        if (normalSprite != null)
            sr.sprite = normalSprite;

        // Возвращаем зону
        skillRangeComp.targetList.targetsInRange.Clear();
        skillRange.SetActive(false);
        normalRange.SetActive(true);
        RefreshTargetList(normalRangeComp, normalRange.GetComponent<BoxCollider2D>());
    }

    void RefreshTargetList()
    {
        RefreshTargetList(skillRangeComp, skillRangeCollider);
    }

    void RefreshTargetList(Range rangeComp, BoxCollider2D col)
    {
        var targets = rangeComp.targetList.targetsInRange;

        Vector2 center = col.transform.position + (Vector3)col.offset;
        Vector2 size = col.size;
        float angle = col.transform.eulerAngles.z;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, angle);

        foreach (var c in colliders)
        {
            EntityData entity = c.GetComponent<EntityData>();
            if (entity == null) continue;
            if (entity.entityType != EntityType.Enemy) continue;
            if (!rangeComp.CanTarget(entity)) continue;
            if (!targets.Contains(entity))
                targets.Add(entity);
        }
    }
}
