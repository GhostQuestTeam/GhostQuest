using HauntedCity.GameMechanics.Main;
using HauntedCity.GameMechanics.SkillSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.Attributes
{
    public class AttributeView:MonoBehaviour
    {
        public PlayerGameStats.PlayerAttributes Attribute;
        public Button Plus;
        public Button Minus;
        public Text Value;

        [Inject] private GameController _gameController;
        
        void Start()
        {
            Plus.onClick.AddListener(() => GameController.GameStats.IncAttribute(Attribute));
            Minus.onClick.AddListener(() => GameController.GameStats.DecAttribute(Attribute));
            _gameController.OnPlayerStatsUpdate += UpdateView;
            UpdateView();
        }
        
        void OnEnable()
        {
            GameController.GameStats.OnAttributeChange += UpdateView;
            _gameController.OnPlayerStatsUpdate += UpdateView;
        }

        void OnDisable()
        {
            GameController.GameStats.OnAttributeChange -= UpdateView;
            _gameController.OnPlayerStatsUpdate -= UpdateView;
        }

        private void UpdateView()
        {
            Value.text = GameController.GameStats.GetAttribute(Attribute).ToString();
        }
        
        
    }
}