using HauntedCity.GameMechanics.Main;
using HauntedCity.GameMechanics.SkillSystem;
using HauntedCity.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.Attributes
{
    public class AttributeView : Panel
    {
        public PlayerCharacteristics Characteristic;
        public Button Plus;
        public Button Minus;
        public Text Value;

        [Inject] private GameController _gameController;

        protected override Model GetModel()
        {
            return GameController.GameStats.CharacteristicManager;
        }
        
        void Start()
        {
            Plus.onClick.AddListener(() =>
                GameController.GameStats.CharacteristicManager.TryIncCharacteristic(Characteristic));
            Minus.onClick.AddListener(() =>
                GameController.GameStats.CharacteristicManager.TryDecCharacteristic(Characteristic));

            Show();
            UpdateView();
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
            Plus.gameObject.SetActive(GameController.GameStats.CharacteristicManager.CanIncCharacteristic(
                Characteristic));
            Minus.gameObject.SetActive(GameController.GameStats.CharacteristicManager.CanDecCharacteristic(
                Characteristic));
            Value.text = GameController.GameStats.CharacteristicManager.GetCharacteristic(Characteristic).ToString();
        }
    }
}