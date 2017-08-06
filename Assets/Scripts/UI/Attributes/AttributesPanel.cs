using System;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking.Interfaces;
using HauntedCity.Utils;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.Attributes
{
    public class AttributesPanel : Panel
    {
        public Button OkButton;
        public Button BackButton;
        public Text UpgradePoints;

        [Inject] private GameController _gameController;
        [Inject] private IPlayerStatsManager _playerStatsManager;

        private void Awake()
        {
        }

        protected override Model GetModel()
        {
            return GameController.GameStats.CharacteristicManager;
        }

        void Start()
        {
            OkButton.onClick.AddListener(() =>
                {
                    //TODO Убрать привязку к конкретным характеристикам
                    _playerStatsManager.UpgradeAttributes(
                        GameController.GameStats.SurvivabilityDelta,
                        GameController.GameStats.EnduranceDelta,
                        GameController.GameStats.PowerDelta
                    );
                    GameController.GameStats.CharacteristicManager.ConfirmUpgrades();
                    ShowInstead(FindObjectOfType<MapPanel>());
                }
            );
        }

        void OnEnable()
        {
            _gameController.OnPlayerStatsUpdate += UpdateView;
        }

        void OnDisable()
        {
            _gameController.OnPlayerStatsUpdate -= UpdateView;
        }

        public override void UpdateView()
        {
            UpgradePoints.text = "Upgrade points: " + GameController.GameStats.CharacteristicManager.UpgradePoints;
        }


        private void OnDestroy()
        {
        }
    }
}