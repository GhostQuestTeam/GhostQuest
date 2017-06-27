using HauntedCity.GameMechanics.Main;
using HauntedCity.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MapPanel : MonoBehaviour
{
    public Text CoinValue;
    public Button SkillsButton;
    public Button StartButton;
    public Animator AttributesPanel;

    [Inject] private ScreenManager _screenManager;
    [Inject] private GameController _gameController;

    void Start()
    {

        GameController.GameStats.OnCoinChange += UpdateCoin;
        
        UpdateLevelView();
        UpdateCoin();
        SkillsButton.onClick.AddListener(() => { _screenManager.OpenPanel(AttributesPanel); });

        StartButton.onClick.AddListener(
            () => _gameController.StartBattle()
        );
    }

    private void OnEnable()
    {
        UpdateLevelView();
    }

    private void UpdateLevelView()
    {
        var level = GameController.GameStats.Level;
        var currentExp = GameController.GameStats.CurrentExp;
        var expToLevel = GameController.GameStats.ExpToLevel;

        GameObject.Find("Level").GetComponent<Text>().text = level.ToString();
        GameObject.Find("Exp").GetComponent<Text>().text = currentExp + "/" + expToLevel;
        GameObject.Find("ExpBar").GetComponent<Image>().fillAmount = (float) currentExp / expToLevel;
    }

    private void UpdateCoin()
    {
        CoinValue.text = GameController.GameStats.Money.ToString();
    }

    private void OnDestroy()
    {
    }
}