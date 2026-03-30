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
            if (UnitUIManager.Instance.panelOpened)
            {
                UnitUIManager.Instance.Close();
            }
            else
            {
                deploySystem.CancelDeploy();
                UnitUIManager.Instance.Open(spawnedUnit, this);

            }
        }
    }

    void UpdateButton()
    {
        button.interactable = cost <= bitiumSystem.bitium;
    }

    void OnDestroy()
    {
        if (bitiumSystem != null)
            bitiumSystem.OnBitiumChanged -= UpdateButton;
    }

    public void SetDeployed(GameObject unitInstance)
    {
        spawnedUnit = unitInstance;
        deployedOverlay.SetActive(true);
        EntityData data = unitInstance.GetComponent<EntityData>();
        data.OnDeath += ClearDeployed;
    }

    public void ClearDeployed()
    {
        if (spawnedUnit != null)
        {
            EntityData data = spawnedUnit.GetComponent<EntityData>();
            if (data != null)
                data.OnDeath -= ClearDeployed;
        }

        spawnedUnit = null;
        deployedOverlay.SetActive(false);
    }
}