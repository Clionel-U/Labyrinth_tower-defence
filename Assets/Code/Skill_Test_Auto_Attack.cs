using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Test_Auto_Attack : Skill
{
    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerAttack;
        activationType = ActivationType.Auto;
        hasDuration = false;
        maxSP = 2f;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnSkillActivate()
    {
        Debug.Log($"{self._name}: скилл активирован (Auto/Attack)");
        Deactivate();
    }

    protected override void OnSkillDeactivate() { }
}