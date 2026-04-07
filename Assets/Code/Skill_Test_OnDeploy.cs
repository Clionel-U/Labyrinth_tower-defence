using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Test_OnDeploy : Skill
{
    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.None;
        activationType = ActivationType.OnDeploy;
        hasDuration = true;
        maxSP = 0f;
        duration = 8f;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnSkillActivate()
    {
        Debug.Log($"{self._name}: скилл активирован при деплое");
    }

    protected override void OnSkillDeactivate()
    {
        Debug.Log($"{self._name}: скилл завершён");
    }
}