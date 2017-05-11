using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class GameController:MonoBehaviour
{
    public static PlayerGameStats GameStats = new PlayerGameStats();

    private BattleStateController _battleStateController;


    void Start()
    {
        _battleStateController = GameObject.Find("BattleStateController").GetComponent<BattleStateController>();
        _battleStateController.OnWon += BattleWonHandle;
    }

    public void StartBattle()
    {
        _battleStateController.StartBattle(new Dictionary<string, int>(){{"skull_ghost", 5}});
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public void InitStats()
    {
        GameStats  = new PlayerGameStats();
    }

    public void BattleWonHandle(int score)
    {
        GameStats.AddExp(score);
        Debug.Log("Level: " + GameStats.Level + "  " + GameStats.CurrentExp + "/" + GameStats.ExpToLevel);
    }

}