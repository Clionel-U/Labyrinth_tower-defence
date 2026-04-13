using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitButton : MonoBehaviour
{

    public GameObject unit;
    public Button button;
    public Image icon;
    public int cost;
    public TMP_Text costText;

    public GameObject spawnedUnit;
    public EntityData spawnedUnitData;
    public GameObject deployedOverlay;   // зелёная виньетка
    public GameObject skillReadyIcon;   // иконка скилла

    public int redeployTime;
    public float redeploymentTimer;
    public bool onCD;

    public void Init(GameObject _unit)
    {   
        deployedOverlay.SetActive(false);
        unit = _unit;

        BitiumSystem.Instance.OnBitiumChanged += UpdateButton;
        spawnedUnitData = unit.GetComponent<EntityData>();
        icon.sprite = spawnedUnitData.icon;
        cost = spawnedUnitData.cost;
        redeployTime = spawnedUnitData.redeployTime;
        costText.text = cost.ToString();
        UpdateButton();
    }
    
    public void OnClick()
    {
        if (spawnedUnit == null)
        {
            UnitUIManager.Instance.Close();
            DeploySystem.Instance.SelectUnit(unit, this);
        }
        else
        {
            UnitUIManager.Instance.Open(spawnedUnit, this);
        }
    }

    void UpdateButton()
    {
        if (DeployLimit.Instance.currentLimit <= 0 && spawnedUnit == null)
        {
            button.interactable = false;
            return;
        }
        if (onCD)
        {
            button.interactable = false;
            return;
        }
        if (spawnedUnit == null)
        button.interactable = cost <= BitiumSystem.Instance.bitium;
    }

    public void SetDeployed(GameObject unitInstance)
    {
        spawnedUnit = Instantiate(unit, unitInstance.transform.position, unitInstance.transform.rotation);
        deployedOverlay.SetActive(true);

        EntityData data = spawnedUnit.GetComponent<EntityData>();
        data.OnDeath += ClearDeployed;

        Vector3 pos = spawnedUnit.transform.position;
        data.occupiedCell = pos;
        GridManager.Instance.occupiedPositions.Add(pos);
        DeployLimit.Instance.MinusLimit();
    }

    public void ClearDeployed()
    {
        spawnedUnitData.OnDeath -= ClearDeployed;

        spawnedUnit = null;
        deployedOverlay.SetActive(false);
        StartRedeployCD();
        DeployLimit.Instance.PlusLimit();
    }

    public void StartRedeployCD()
    {
        onCD = true;
        redeploymentTimer = redeployTime;
        UpdateButton();
    }

    void Update()
    {
        if (!onCD) return;

        redeploymentTimer -= Time.deltaTime;
        if (redeploymentTimer <= 0)
        {
            onCD = false;
            UpdateButton();
        }
    }
}