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

        unitData = currentUnit.GetComponent<EntityData>();
        _name.text = $"{unitData._name}";
        atk.text = $"{unitData.ATK}";
        def.text = $"{unitData.DEF}";
        res.text = $"{unitData.RES}";
        hp.text = $"{unitData.HP}/{unitData.maxHP}";
        hpSlider.maxValue = unitData.maxHP;
        hpSlider.value = unitData.HP;
        unitData.OnHPChanged += UpdateHP;

        switch (unitData.unitID)
        {
            case 1:
                skill.text = "Арсенал двойников";
                skillDesc.text = @"АТК +0%, атакует дважды, первая атака наносит физический урон, вторая атака наносит магический урон";
                break;
            case 2:
                skill.text = "Метаморфоза";
                skillDesc.text = @"Перестает блокировать врагов, АТК +0%, область атаки изменяется, теперь может атаковать воздушных врагов";
                break;
            case 3:
                skill.text = "Всемирная пытка";
                skillDesc.text = @"Все враги на карте 3 раза получают физический урон в размере 60% от АТК";
                break;
            case 4:
                skill.text = "Препарат NT-X7";
                skillDesc.text = @"Бросает препарат во врага, замедляя его и отравляя (наносит магический урон в размере % от АТК каждую секунду) в течении 3 секунд";
                break;
            case 5:
                skill.text = "Раздор";
                skillDesc.text = @"Пассивно: вместо атаки накладывает на врагов в области атаки эффект ""Разлад"" (наносит урон в размере % от АТК каждые секунды)
Активно: все враги на карте получают единоразовый магический урон в размере % от АТК и эффект ""Разлад"" на всю длительность навыка, теперь может атаковать врагов в области атаки";
                break;
            case 6:
                skill.text = "Нейросимфония";
                skillDesc.text = @"Увеличивает эффективность лечения юнитов в области навыка на 20%, +20% АТК всем юнитам в области навыка";
                break;
            case 7:
                skill.text = "Обострение";
                skillDesc.text = @"Область атаки уменьшается, Интервал атаки -0%";
                break;
            case 8:
                skill.text = "Магические пули";
                skillDesc.text = @"Юнит использует магические пули, чтобы поражать все, что окажется в его поле зрении:
Пули 1-6: Стреляет во врага с самой низкой защитой, нанося ему физический урон в размере 200% АТК * Пуля.
Пуля 7: Стреляет в случайного юнита, нанося ему урон в размере 60% от максимального ХП цели + 100% АТК
После 7-ой пули счетчик пуль сбрасывается, АТК +0%
После выстрела +1 пуля";
                break;
            case 9:
                skill.text = "Подрыв";
                skillDesc.text = @"Все враги в области атаки получают магический урон в размере 0% от АТК и останавливаются на 0 секунд";
                break;

        }

        panel.SetActive(true);
        panelOpened = true;
    }

    public void Close()
    {
        panel.SetActive(false);
        currentUnit = null;
        currentButton = null;
        panelOpened = false;
        unitData.OnHPChanged -= UpdateHP;
        unitData = null;

    }

    public void UpdateHP(int current, int max)
    {

        hp.text = $"{current}/{max}";
        hpSlider.value = current;
    }

    public void UseSkill()
    {
        if (currentUnit == null) return;

        Skill skill = currentUnit.GetComponent<Skill>();
        if (skill != null && skill.activationType != ActivationType.OnDeploy)
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
