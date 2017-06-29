using HauntedCity.GameMechanics.Main;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.Attributes
{
    public class AttributesPanel : Panel
    {
        public Button OkButton;
        public Button BackButton;
        public Text UpgradePoints;

        [Inject] private ScreenManager _screenManager;
  
        void Start()
        {            
            OkButton.onClick.AddListener(() =>
                {
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
        }

        void OnDisable()
        {
            GameController.GameStats.OnAttributeChange -= _UpdateView;
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