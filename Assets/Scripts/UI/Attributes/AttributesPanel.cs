using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking.Interfaces;
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
        
        void Start()
        {            
            OkButton.onClick.AddListener(() =>
                {
                    
                    _playerStatsManager.UpgradeAttributes(
                         GameController.GameStats.SurvivabilityDelta,
                         GameController.GameStats.EnduranceDelta,
                         GameController.GameStats.PowerDelta
                     );
                    GameController.GameStats.ConfirmUpgrades();
                    ShowInstead(FindObjectOfType<MapPanel>());
                }
            );
        }

        protected override void OnShow()
        {
            _UpdateView();
        }
        
        void OnEnable()
        {
            GameController.GameStats.OnAttributeChange += _UpdateView;
            _gameController.OnPlayerStatsUpdate += _UpdateView;
        }

        void OnDisable()
        {
            GameController.GameStats.OnAttributeChange -= _UpdateView;
            _gameController.OnPlayerStatsUpdate -= _UpdateView;
        }

        private void _UpdateView()
        {
            UpgradePoints.text = "Upgrade points: " + GameController.GameStats.UpgradePoints;
        }


        private void OnDestroy()
        {
        }
    }
}