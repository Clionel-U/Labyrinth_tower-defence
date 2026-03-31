using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitButton : MonoBehaviour
{
    public Image icon;

    public GameObject unit;
    public DeploySystem deploySystem;
    public BitiumSystem bitiumSystem;
    public Button button;
    public int cost;
    public TMP_Text costText;

    public GameObject spawnedUnit;
    public GameObject deployedOverlay;   // зелёная виньетка
    public GameObject skillReadyIcon;   // иконка скилла

    public int redeployTime;
    public float redeploymentTimer;
    public bool onCD;

    public void Init(GameObject _unit, DeploySystem depSys, BitiumSystem bitSys)
    {   
        deployedOverlay.SetActive(false);
        unit = _unit;
        deploySystem = depSys;
        bitiumSystem = bitSys;
        bitiumSystem.OnBitiumChanged += UpdateButton;
        EntityData data = unit.GetComponent<EntityData>();
        icon.sprite = data.icon;
        cost = data.cost;
        redeployTime = data.redeployTime;
        costText.text = cost.ToString();
        UpdateButton();
    }
    
    public void OnClick()
    {
        if (spawnedUnit == null)
        {
            UnitUIManager.Instance.Close();
            deploySystem.SelectUnit(unit, this);
        }
        else
        {
            UnitUIManager.Instance.Open(spawnedUnit, this);
        }
    }

    void UpdateButton()
    {
        if (onCD)
        {
            button.interactable = false;
            return;
        }
        if (spawnedUnit == null)
        button.interactable = cost <= bitiumSystem.bitium;
    }

    public void SetDeployed(GameObject unitInstance)
    {
        spawnedUnit = unitInstance;
        deployedOverlay.SetActive(true);
        EntityData data = spawnedUnit.GetComponent<EntityData>();
        data.OnDeath += ClearDeployed;
    }

    public void ClearDeployed()
    {
        EntityData data;

        if (spawnedUnit != null)
        {
            data = spawnedUnit.GetComponent<EntityData>();
            if (data != null)
            {
                data.OnDeath -= ClearDeployed;
            }
        }
       
        spawnedUnit = null;
        deployedOverlay.SetActive(false);
        StartRedeployCD();
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