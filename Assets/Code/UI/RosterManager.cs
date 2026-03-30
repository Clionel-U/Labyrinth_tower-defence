using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RosterManager : MonoBehaviour
{
    public Transform rosterContainer;
    public GameObject buttonPrefab;
    public DeploySystem deploySystem;
    public BitiumSystem bitiumSystem;
    public SelectionList selectionList;

    void Start()
    {
        BuildRoster(selectionList.selectedUnits);
    }
    void BuildRoster(List<GameObject> selectedUnits)
    {
        // очистка старых кнопок
        foreach (Transform child in rosterContainer)
        {
            Destroy(child.gameObject);
        }

        // сортировка
        var sorted = selectedUnits
        .Select(u => new { unit = u, data = u.GetComponent<EntityData>() })
        .OrderBy(x => x.data.cost)
        .ThenBy(x => x.data._name)
        .Select(x => x.unit)
        .ToList();

        // создание кнопок
        foreach (var unit in sorted)
        {
            GameObject btnObj = Instantiate(buttonPrefab, rosterContainer);

            UnitButton btn = btnObj.GetComponent<UnitButton>();
            btn.Init(unit, deploySystem, bitiumSystem);
        }
    }
}