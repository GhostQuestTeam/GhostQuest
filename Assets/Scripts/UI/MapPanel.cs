using HauntedCity.GameMechanics.Main;
using HauntedCity.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MapPanel : MonoBehaviour
{
    public Button SkillsButton;
    public Button StartButton;
    public Animator AttributesPanel;

    [Inject] private ScreenManager _screenManager;
    [Inject] private GameController _gameController;

    void Start()
    {

        _UpdateLevelView();
        SkillsButton.onClick.AddListener(() => { _screenManager.OpenPanel(AttributesPanel); });

        StartButton.onClick.AddListener(
            () => _gameController.StartBattle()
        );
    }

    private void OnEnable()
    {
        _UpdateLevelView();
    }

    private void _UpdateLevelView()
    {
        var level = GameController.GameStats.Level;
        var currentExp = GameController.GameStats.CurrentExp;
        var expToLevel = GameController.GameStats.ExpToLevel;

        GameObject.Find("Level").GetComponent<Text>().text = level.ToString();
        GameObject.Find("Exp").GetComponent<Text>().text = currentExp + "/" + expToLevel;
        GameObject.Find("ExpBar").GetComponent<Image>().fillAmount = (float) currentExp / expToLevel;
    }

    private void OnDestroy()
    {
    }
}