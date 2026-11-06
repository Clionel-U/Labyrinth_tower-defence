using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUIManager : MonoBehaviour
{
    public static UnitUIManager Instance;

    public GameObject panel;
    public GameObject skillButtonObj;
    public Button skillButton;
    public GameObject retreatButtonObj;
    public Button retreatButton;

    private GameObject currentUnit;
    private UnitButton currentButton;
    private Skill currentSkill;
    public bool panelOpened = false;

    private EntityData unitData;
    public TMP_Text _name;
    public TMP_Text atk;
    public TMP_Text def;
    public TMP_Text res;
    public TMP_Text hp;
    public Slider hpSlider;
    public TMP_Text skill;
    public TMP_Text skillDesc;
    public Image skillIcon;
    public List<Sprite> skillIcons = new List<Sprite>();
    public TMP_Text sp;


    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Open(GameObject unit, UnitButton button, bool isDeployed)
    {
        if (isDeployed)
        {
            skillButtonObj.SetActive(true);
            retreatButtonObj.SetActive(true);
        }
        else
        {
            skillButtonObj.SetActive(false);
            retreatButtonObj.SetActive(false);
        }
        if (unit == currentUnit)
        {
            Close();
            return;
        }
        currentUnit = unit;
        currentButton = button;
        currentSkill = currentUnit.GetComponent<Skill>();
        unitData = currentUnit.GetComponent<EntityData>();
        _name.text = $"{unitData._name}";
        atk.text = $"{unitData.ATK}";
        def.text = $"{unitData.DEF}";
        res.text = $"{unitData.RES}";
        hp.text = $"{unitData.HP}/{unitData.maxHP}";
        hpSlider.maxValue = unitData.maxHP;
        hpSlider.value = unitData.HP;
        unitData.OnHPChanged += UpdateHP;
        sp.text = $"{Mathf.RoundToInt(currentSkill.currentSP)}/{currentSkill.maxSP}";
        currentSkill.OnSPChanged += UpdateSP;
        if (currentSkill.activationType == ActivationType.Auto || currentSkill.currentSP < currentSkill.maxSP) skillButton.interactable = false;

        switch (unitData.unitID)
        {
            case 1:
                skill.text = "������� ���������";
                skillDesc.text = @"��� +0%, ������� ������, ������ ����� ������� ���������� ����, ������ ����� ������� ���������� ����";
                skillIcon.sprite = skillIcons[0];
                break;
            case 2:
                skill.text = "�����������";
                skillDesc.text = @"��������� ����������� ������, ��� +0%, ������� ����� ����������, ������ ����� ��������� ��������� ������";
                skillIcon.sprite = skillIcons[1];
                break;
            case 3:
                skill.text = "��������� �����";
                skillDesc.text = @"��� ����� �� ����� 3 ���� �������� ���������� ���� � ������� 60% �� ���";
                skillIcon.sprite = skillIcons[2];
                break;
            case 4:
                skill.text = "�������� NT-X7";
                skillDesc.text = @"������� �������� �� �����, �������� ��� � �������� (������� ���������� ���� � ������� % �� ��� ������ �������) � ������� 3 ������";
                skillIcon.sprite = skillIcons[3];
                break;
            case 5:
                skill.text = "������";
                skillDesc.text = @"��������: ������ ����� ����������� �� ������ � ������� ����� ������ ""������"" (������� ���� � ������� % �� ��� ������ �������)
�������: ��� ����� �� ����� �������� ������������ ���������� ���� � ������� % �� ��� � ������ ""������"" �� ��� ������������ ������, ������ ����� ��������� ������ � ������� �����";
                skillIcon.sprite = skillIcons[4];
                break;
            case 6:
                skill.text = "�������������";
                skillDesc.text = @"����������� ������������� ������� ������ � ������� ������ �� 20%, +20% ��� ���� ������ � ������� ������";
                skillIcon.sprite = skillIcons[5];
                break;
            case 7:
                skill.text = "����������";
                skillDesc.text = @"������� ����� �����������, �������� ����� -0%";
                skillIcon.sprite = skillIcons[6];
                break;
            case 8:
                skill.text = "���������� ����";
                skillDesc.text = @"���� ���������� ���������� ����, ����� �������� ���, ��� �������� � ��� ���� ������:
���� 1-6: �������� �� ����� � ����� ������ �������, ������ ��� ���������� ���� � ������� 200% ��� * ����.
���� 7: �������� � ���������� �����, ������ ��� ���� � ������� 60% �� ������������� �� ���� + 100% ���
����� 7-�� ���� ������� ���� ������������, ��� +0%
����� �������� +1 ����";
                skillIcon.sprite = skillIcons[7];
                break;
            case 9:
                skill.text = "������";
                skillDesc.text = @"��� ����� � ������� ����� �������� ���������� ���� � ������� 0% �� ��� � ��������������� �� 0 ������";
                skillIcon.sprite = skillIcons[8];
                break;

        }

        panel.SetActive(true);
        panelOpened = true;
    }

    public void Close()
    {
        if (!panelOpened) return;
        if (currentUnit != null) unitData.OnHPChanged -= UpdateHP;
        if (currentSkill != null) currentSkill.OnSPChanged -= UpdateSP;

        panel.SetActive(false);
        currentUnit = null;
        currentButton = null;
        panelOpened = false;
        currentSkill = null;
        unitData = null;

    }

    public void UpdateHP(int current, int max)
    {
        if (hp == null) return;
        hp.text = $"{current}/{max}";
        hpSlider.value = current;
    }

    public void UpdateSP()
    {
        sp.text = $"{Mathf.RoundToInt(currentSkill.currentSP)}/{currentSkill.maxSP}";
        if (currentSkill.currentSP >= currentSkill.maxSP && currentSkill.activationType == ActivationType.Manual)
            skillButton.interactable = true;
        else if (currentSkill.canToggleOff && currentSkill.isActive)
            skillButton.interactable = true;
        else skillButton.interactable = false;
    }

    public void UseSkill()
    {
        if (currentUnit == null) return;

        if (currentSkill != null && currentSkill.activationType != ActivationType.OnDeploy)
        {
            currentSkill.Activate(); 
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
