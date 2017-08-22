using HauntedCity.GameMechanics.Main;
using HauntedCity.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        UpdateLevelView();
        UpdateCoin();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnEnable()
    {
        _gameController.OnPlayerStatsUpdate += UpdateView;
        CheckDied();

        UpdateLevelView();
    }

    private void OnDisable()
    {
        _gameController.OnPlayerStatsUpdate -= UpdateView;
    }

    public override void UpdateView()
    {
        UpdateLevelView();
        UpdateCoin();
        CheckDied();
    }

    private void CheckDied()
    {
        if (!GameController.GameStats.IsAlive)
        {
            FindObjectOfType<DiedPanel>().Show();
        }
        else
        {
            FindObjectOfType<DiedPanel>().Hide();
        }
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
    
    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "battle")
        {
            Hide();
        }
    }
}