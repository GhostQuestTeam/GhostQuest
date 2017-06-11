using System;
using HauntedCity.GameMechanics.Main;
using HauntedCity.GameMechanics.SkillSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class AttributesPanel : MonoBehaviour
    {
        public Button OkButton;
        public Button BackButton;
        public Text UpgradePoints;

        public Animator PrevPanel;

        private ScreenManager _screenManager;

        void Start()
        {
            _screenManager = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
            
            OkButton.onClick.AddListener(() =>
                {
                    GameController.GameStats.ConfirmUpgrades();
                    _screenManager.OpenPanel(PrevPanel);
                }
            );
            BackButton.onClick.AddListener(() => _screenManager.OpenPanel(PrevPanel));
        }

        void OnEnable()
        {
            GameController.GameStats.OnAttributeChange += _UpdateView;
            _UpdateView();
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