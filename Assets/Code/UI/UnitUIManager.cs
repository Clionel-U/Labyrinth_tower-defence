using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager : MonoBehaviour
{
    public static UnitUIManager Instance;

    public GameObject panel;
    public Button skillButton;
    public Button retreatButton;

    private GameObject currentUnit;
    private UnitButton currentButton;
    public bool panelOpened = false;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Open(GameObject unit, UnitButton button)
    {
        if (unit == currentUnit)
        {
            Close();
            return;
        }
        currentUnit = unit;
        currentButton = button;

        panel.SetActive(true);
        panelOpened = true;
    }

    public void Close()
    {
        panel.SetActive(false);
        currentUnit = null;
        currentButton = null;
        panelOpened = false;
    }

    public void UseSkill()
    {
        if (currentUnit == null) return;

        Skill skill = currentUnit.GetComponent<Skill>();
        if (skill != null)
        {
            skill.Activate(); 
        }
    }

    public void Retreat()
    {
        if (currentUnit == null) return;

        currentButton.ClearDeployed();
        Destroy(currentUnit);

        Close();
    }
}
