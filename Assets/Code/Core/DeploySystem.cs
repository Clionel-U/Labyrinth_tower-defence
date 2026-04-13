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
    public static DeploySystem Instance;
     void Awake() => Instance = this;

    public Camera cam;
    public Button confirmButton;
    public GameObject dirButtonsPrefab;
    public GameObject previewUnitPrefab;

    [SerializeField] private DeployState state = DeployState.None;
    [SerializeField] private GameObject selectedUnit;
    private UnitButton currentButton;
    [SerializeField] private GameObject previewUnit;
    [SerializeField] private GameObject dirButtons;
    [SerializeField] private UnitType unitType;
    [SerializeField] private int cost;


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
        else if (state != DeployState.None) CancelDeploy();
            selectedUnit = unitPrefab;
        currentButton = button;

        EntityData selUnitData = selectedUnit.GetComponent<EntityData>();
        unitType = selUnitData.unitType;
        cost = selUnitData.cost;

        TileHighlighter.Instance.ShowHighlights(unitType);

        previewUnitPrefab.GetComponent<SpriteRenderer>().sprite = selectedUnit.GetComponent<SpriteRenderer>().sprite;

        state = DeployState.SelectingTile;
        Debug.Log("Выбор клетки");
    }

    void Update()
    {
        if (state == DeployState.SelectingTile)
        {
            TileSelection();
        }
        if (state != DeployState.None && cost > BitiumSystem.Instance.bitium)
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
            if ((unitType == UnitType.Melee && GridManager.Instance.IsGround(pos)) || (unitType == UnitType.Ranged && GridManager.Instance.IsHighGround(pos))) 
            {
                pos = GridManager.Instance.GetCellCenter(pos);
                if (GridManager.Instance.occupiedPositions.Contains(pos)) return;
                
                previewUnit = Instantiate(previewUnitPrefab, pos, Quaternion.identity);

                state = DeployState.SelectingDirection;
                DirectionSelection(pos);
                Debug.Log("Выбор направления");
            }
        }
    }

    void DirectionSelection(Vector3 pos)
    {
        if (previewUnit == null) return;
        
        dirButtons = Instantiate(dirButtonsPrefab, pos, Quaternion.identity);
        Rotator rotator = dirButtons.GetComponent<Rotator>();
        rotator.previewUnit = previewUnit;

        TileHighlighter.Instance.Clear();
        
        confirmButton.gameObject.SetActive(true);
    }

    public void ConfirmDeploy()
    {
        if (previewUnit == null) return;

        currentButton.SetDeployed(previewUnit);
        BitiumSystem.Instance.bitium -= cost;
        BitiumSystem.Instance.BitiumChange();
        
        Destroy(previewUnit);
        previewUnit = null;
        currentButton = null;

        state = DeployState.None;
        confirmButton.gameObject.SetActive(false);
        TileHighlighter.Instance.Clear();
        Destroy(dirButtons);
        Debug.Log("Юнит установлен");
        UnitUIManager.Instance.Close();
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
        TileHighlighter.Instance.Clear();
        Debug.Log("Отмена деплоя");
    }
}