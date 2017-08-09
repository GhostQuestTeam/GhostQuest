using HauntedCity.GameMechanics.Main;
using HauntedCity.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MapPanel : Panel
{
    public Text CoinValue;
    public Animator AttributesPanel;

    [Inject] private GameController _gameController;

    void Start()
    {

        GameController.GameStats.OnCoinChange += UpdateCoin;
        
        UpdateLevelView();
        UpdateCoin();

    }

    private void OnEnable()
    {
        _gameController.OnPlayerStatsUpdate += UpdateView;
        UpdateLevelView();
    }

    private void OnDisable()
    {
        _gameController.OnPlayerStatsUpdate -= UpdateView;
    }

    private void UpdateView()
    {
        UpdateLevelView();
        UpdateCoin();
    }

    private void UpdateLevelView()
    {
        var level = GameController.GameStats.PlayerExperience.Level;
        var currentExp = GameController.GameStats.PlayerExperience.CurrentExp;
        var expToLevel = GameController.GameStats.PlayerExperience.ExpToLevel;

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