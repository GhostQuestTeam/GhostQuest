using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
   
    public const string DEFAULT_UI_PANEL_PREFAB = "UIFightPanel";
    public const string DEFAULT_ATTRIBUTES_PANEL_PREFAB = "AttributesPanel";
    public const string DEFAULT_START_BATTLE_PREFAB = "BattleSystem/UI/StartBattleButton";

  
    public string UIPanelPrefabPath = DEFAULT_UI_PANEL_PREFAB;
    public string StartBattlePrefabPath = DEFAULT_START_BATTLE_PREFAB;
    public string AttributesPanelPrefabPath = DEFAULT_ATTRIBUTES_PANEL_PREFAB;

   
    private GameObject _uiPanel;
    private Transform _canvas;

    private GameObject _startBattleButton;

    // Use this for initialization
    void Start()
    {
        _canvas = transform.parent;

        _initNavBar();

        _canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void _initNavBar()
    {
        var uiPanelPrefab = Resources.Load(UIPanelPrefabPath) as GameObject;
        _uiPanel = Instantiate(uiPanelPrefab);

        _uiPanel.transform.SetParent(_canvas, false);
    }

    // Update is called once per frame
    void Update()
    {
    }
}