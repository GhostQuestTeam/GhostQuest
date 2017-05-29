using SkillSystem;
using UnityEngine;
using UnityEngine.UI;

public class MapUIController:MonoBehaviour
{
    private GameObject _attributesPanel;
    private GameObject _mainPanel;

    void Start()
    {
        _attributesPanel = GameObject.Find("AttributesPanel");
        _mainPanel = GameObject.Find("MainPanel");

        _UpdateLevelView();
        GameObject.Find("Skills").GetComponent<Button>().onClick.AddListener(() =>
        {
            _attributesPanel.SetActive(true);
            _mainPanel.SetActive(false);
            
        });

        var gameController = GameObject.Find("GameController").GetComponent<GameController>();

        GameObject.Find("StartBattle").GetComponent<Button>().onClick.AddListener(
                () => gameController.StartBattle()
        );

        //GameObject.Find("StartBattle").SetActive(false);

        _attributesPanel.GetComponent<AttributesView>().OnClose += CloseHandler;

        _attributesPanel.SetActive(false);
    }

    private void _UpdateLevelView()
    {
        var level = GameController.GameStats.Level;
        var currentExp = GameController.GameStats.CurrentExp;
        var expToLevel = GameController.GameStats.ExpToLevel;

        GameObject.Find("Level").GetComponent<Text>().text = level.ToString();
        GameObject.Find("Exp").GetComponent<Text>().text = currentExp + "/" + expToLevel;
        GameObject.Find("ExpBar").GetComponent<Image>().fillAmount = (float)currentExp/ expToLevel;
    }

    private void OnDestroy()
    {
       _attributesPanel.GetComponent<AttributesView>().OnClose -= CloseHandler;
    }

    void CloseHandler()
    {
        _attributesPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }
}