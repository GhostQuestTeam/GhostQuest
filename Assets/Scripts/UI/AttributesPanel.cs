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

        void Close()
        {
            gameObject.SetActive(false);
            _screenManager.OpenPanel(PrevPanel);
        }
        
        void Start()
        {
            _screenManager = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
            
            OkButton.onClick.AddListener(() =>
                {
                    GameController.GameStats.ConfirmUpgrades();
                    Close();
                }
            );
            BackButton.onClick.AddListener(Close);
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