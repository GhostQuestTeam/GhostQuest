﻿using System;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using HauntedCity.UI.PointInfo;
using HauntedCity.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class BattleResultPanel:Panel
    {
        public Text Result;
        public GhostsPanel Ghosts;
        public Text EarnedExp;

        [Inject] private GameController _gameController;

        private void Start()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (oldScene.name == "battle")
            {
                Show();
            }
        }

        public override void UpdateView()
        {
            var battleResult = _gameController.LastBattleResult;
            switch (battleResult.Type)
            {
                case BattleStateController.BattleResultType.WON:
                    Result.text = "You won!";
                    break;
                case BattleStateController.BattleResultType.LOSE:
                    Result.text = "You died!";
                    break;
                case BattleStateController.BattleResultType.STOPED:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            EarnedExp.text = battleResult.EarnedExp.ToString();
            Ghosts.UpdateView(battleResult.KilledEnemies);
        }
    }
}