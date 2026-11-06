using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillVortex : Skill
{
    [Header("��������� ����� ���")]
    public GameObject vortexPrefab;
    public float damageMultiplier;

    private GameObject currentVortex;

    protected override void Awake()
    {
        base.Awake();
        spChargeType = SPChargeType.PerSecond;
        activationType = ActivationType.Manual;
        hasDuration = true;
        duration = 20f;
        maxSP = 30f;
    }

    protected override void OnSkillActivate()
    {
        // ������� ����� �� ������� �����
        currentVortex = Instantiate(vortexPrefab, transform.position, Quaternion.identity);
        currentVortex.transform.SetParent(transform);

        // ������ ��������� ����� ����� ������ ������� �����
        var vortexScript = currentVortex.GetComponent<VortexLogic>();
        if (vortexScript != null)
        {
            vortexScript.vortexAtk = Mathf.RoundToInt(self.ATK * damageMultiplier);
        }
    }

    protected override void OnSkillDeactivate()
    {
        // ���������� ����� ��� ����������� ������
        if (currentVortex != null)
        {
            Destroy(currentVortex);
        }
    }
}
