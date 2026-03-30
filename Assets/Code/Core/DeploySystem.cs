using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum DeployState
{
    None,
    SelectingTile,
    SelectingDirection
}

public class DeploySystem : MonoBehaviour
{
    public Camera cam;
    public GridManager grid;
    public TileHighlighter highlighter;
    public Button confirmButton;
    public GameObject dirButtonsPrefab;
    public BitiumSystem bitiumSystem;

    [SerializeField] private DeployState state = DeployState.None;
    [SerializeField] private GameObject selectedUnit;
    [SerializeField] private GameObject previewUnit;
    [SerializeField] private GameObject dirButtons;
    [SerializeField] private UnitType unitType;
    [SerializeField] private int cost;
    private UnitButton currentButton;

    void Start()
    {
        confirmButton.gameObject.SetActive(false);
    }

    public void SelectUnit(GameObject unitPrefab, UnitButton button)
    {
        // повторный клик = отмена
        if (state != DeployState.None && selectedUnit == unitPrefab)
        {
            CancelDeploy();
            return;
        }
        selectedUnit = unitPrefab;
        currentButton = button;
        unitType = unitPrefab.GetComponent<EntityData>().unitType;
        cost = unitPrefab.GetComponent<EntityData>().cost;
        highlighter.ShowHighlights(unitType);
        
        state = DeployState.SelectingTile;

        Debug.Log("Выбор клетки");
    }
    void Update()
    {
        if (state == DeployState.SelectingTile)
        {
            TileSelection();
        }
        if (state != DeployState.None && cost > bitiumSystem.bitium)
        {
            CancelDeploy();
        }
    }

    void TileSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            if ((unitType == UnitType.Melee && grid.IsGround(pos)) || (unitType == UnitType.Ranged && grid.IsHighGround(pos))) 
            {
                pos = grid.GetCellCenter(pos);
                if (grid.occupiedPositions.Contains(pos)) return;
                //selectedPosition = pos;
                previewUnit = Instantiate(selectedUnit, pos, Quaternion.identity);

                state = DeployState.SelectingDirection;
                DirectionSelection();
                Debug.Log("Выбор направления");
            }
        }
    }

    void DirectionSelection()
    {
        if (previewUnit == null) return;
        Vector3 pos = previewUnit.transform.position;
        dirButtons = Instantiate(dirButtonsPrefab, pos, Quaternion.identity);
        Rotator rotator = dirButtons.GetComponent<Rotator>();
        rotator.previewUnit = previewUnit;
        highlighter.Clear();
        confirmButton.gameObject.SetActive(true);
    }

    public void ConfirmDeploy()
    {
        if (previewUnit == null)
        {
            return;
        }
        currentButton.SetDeployed(previewUnit);
        bitiumSystem.bitium -= cost;
        bitiumSystem.BitiumChange();
        grid.occupiedPositions.Add(previewUnit.transform.position);
        previewUnit = null;
        currentButton = null;
        state = DeployState.None;
        confirmButton.gameObject.SetActive(false);
        highlighter.Clear();
        Destroy(dirButtons);
        Debug.Log("Юнит установлен");
    }

    public void CancelDeploy()
    {
        if (previewUnit != null)
        {
            Destroy(previewUnit);
        }
        if (dirButtons != null)
        {
            Destroy(dirButtons);
        }
        state = DeployState.None;
        confirmButton.gameObject.SetActive(false);
        highlighter.Clear();
        Debug.Log("Отмена деплоя");
    }
}