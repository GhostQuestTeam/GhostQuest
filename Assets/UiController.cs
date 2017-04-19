using System;
using DialogueSystem;
using QuestSystem;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public const string DEFAULT_DIALOGUE_PANEL_PREFAB = "DialoguesUI/DialoguePanel";
    public const string DEFAULT_QUEST_PANEL_PREFAB = "QuestsUI/QuestPanel";
    public const string DEFAULT_UI_PANEL_PREFAB = "UIFightPanel";

    public string DialoguePanelPrefabPath = DEFAULT_DIALOGUE_PANEL_PREFAB;
    public string QuestPanelPrefabPath = DEFAULT_QUEST_PANEL_PREFAB;
    public string UIPanelPrefabPath = DEFAULT_UI_PANEL_PREFAB;

    private GameObject _dialoguePanel;
    private GameObject _questPanel;
    private GameObject _uiPanel;
    private Transform _canvas;

    // Use this for initialization
    void Start()
    {


        _canvas = transform.parent;

        _initDialoguePanel();
        _initQuestPanel();
        _initNavBar();
        _initEvents();

        _canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public GameObject DialoguePanel
    {
        get { return _dialoguePanel; }
    }

    private void _initDialoguePanel()
    {
        var dialoguePanelPrefab = Resources.Load(DialoguePanelPrefabPath) as GameObject;
        _dialoguePanel = Instantiate(dialoguePanelPrefab);
        _dialoguePanel.transform.SetParent(_canvas, false);
    }

    private void _initQuestPanel()
    {
        var questPanelPrefab = Resources.Load(QuestPanelPrefabPath) as GameObject;
        _questPanel = Instantiate(questPanelPrefab);

        _questPanel.transform.SetParent(_canvas, false);
        var exitButton = _questPanel.transform.FindChild("HeadPanel").Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(() => _questPanel.GetComponent<QuestView>().Close());
    }

    private void _initNavBar()
    {
        var uiPanelPrefab = Resources.Load(UIPanelPrefabPath) as GameObject;
        _uiPanel = Instantiate(uiPanelPrefab);

        _uiPanel.transform.SetParent(_canvas, false);

        var questsButton = _uiPanel.transform.Find("ButtonQuests");
        questsButton.GetComponent<Button>().onClick.AddListener(_questsClickHandler);
    }

    private void _initEvents()
    {
        _questPanel.GetComponent<QuestView>().OnClose += _questsCloseHandler;
    }

    private void _questsClickHandler()
    {
        _questPanel.SetActive(true);
        _questPanel.GetComponent<QuestView>().Draw();
        _uiPanel.gameObject.SetActive(false);
    }

    private void _questsCloseHandler()
    {
        _uiPanel.gameObject.SetActive(true);
    }

// Update is called once per frame
    void Update()
    {
    }
}