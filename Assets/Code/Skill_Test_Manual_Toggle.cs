using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Test_Manual_Toggle : Skill
{
    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerAttack;
        activationType = ActivationType.Manual;
        hasDuration = false;
        canToggleOff = true;
        maxSP = 8f;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnSkillActivate()
    {
        Debug.Log($"{self._name}: скилл включён (Toggle ON)");
    }

    protected override void OnSkillDeactivate()
    {
        Debug.Log($"{self._name}: скилл выключен (Toggle OFF)");
    }
}
