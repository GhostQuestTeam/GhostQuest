using System;
using HauntedCity.GameMechanics.Main;
using HauntedCity.GameMechanics.SkillSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
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
                    Hide();
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