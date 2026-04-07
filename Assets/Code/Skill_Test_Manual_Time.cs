using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Test_Manual_Time : Skill
{
    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = true;
        maxSP = 10f;
        duration = 5f;
    }

    protected override void Update()
    {
        base.Update();
        if (!isActive)
            ChargePerSecond();
    }

    protected override void OnSkillActivate()
    {
        Debug.Log($"{self._name}: скилл активирован (Manual/Time)");
    }

    protected override void OnSkillDeactivate()
    {
        Debug.Log($"{self._name}: скилл завершён");
    }
}
