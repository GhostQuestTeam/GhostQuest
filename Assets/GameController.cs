using System.Collections.Generic;
using BattleSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController:MonoBehaviour
{
    public static PlayerGameStats GameStats = new PlayerGameStats();

    private BattleStateController _battleStateController;


    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
        _battleStateController = GameObject.Find("BattleStateController").GetComponent<BattleStateController>();
        _battleStateController.OnWon += BattleWonHandle;
        _battleStateController.OnLose += BattleLoseHandle;

    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _battleStateController.StartBattle(new Dictionary<string, int>(){{"skull_ghost", 5}});
        }

    }

    public void StartBattle()
    {
        SceneManager.LoadScene(1);

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public void InitStats()
    {
        GameStats  = new PlayerGameStats();
    }

    public void BattleWonHandle(int score)
    {
        GameStats.AddExp(score);
        SceneManager.LoadScene(0);
        Debug.Log("Level: " + GameStats.Level + "  " + GameStats.CurrentExp + "/" + GameStats.ExpToLevel);
    }

    public void BattleLoseHandle()
    {
        SceneManager.LoadScene(0);
    }

}