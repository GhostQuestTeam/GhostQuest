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
        SceneManager.sceneLoaded += OnSceneLoad;

    }

    void Start()
    {


    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
        if (scene.buildIndex == 2)
        {
            _battleStateController.StartBattle(new Dictionary<string, int>(){{"skull_ghost", 5}});
        }
    }


    public void StartBattle()
    {
        SceneManager.LoadScene(2);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public void InitStats()
    {
        GameStats  = new PlayerGameStats();
    }

    public void BattleWonHandle(int score)
    {
        GameStats.AddExp(score);
        SceneManager.LoadScene(1);
        Debug.Log("Level: " + GameStats.Level + "  " + GameStats.CurrentExp + "/" + GameStats.ExpToLevel);
    }

    public void BattleLoseHandle()
    {
        SceneManager.LoadScene(1);
    }

}