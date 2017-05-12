using SkillSystem;
using UnityEngine;
using UnityEngine.UI;

public class MapUIController:MonoBehaviour
{
    private GameObject _attributesPanel;
    private GameObject _skillsButton;
    private GameObject _levelPanel;

    void Start()
    {
        _attributesPanel = GameObject.Find("AttributesPanel");
        _skillsButton = GameObject.Find("Skills");
        _levelPanel = GameObject.Find("LevelPanel");

        _UpdateLevelView();
        _skillsButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            _attributesPanel.SetActive(true);
            _skillsButton.SetActive(false);
            _levelPanel.SetActive(false);
        });

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
        _skillsButton.SetActive(true);
        _levelPanel.SetActive(true);
    }
}