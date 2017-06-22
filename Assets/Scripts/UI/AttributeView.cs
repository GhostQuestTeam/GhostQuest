using HauntedCity.GameMechanics.Main;
using HauntedCity.GameMechanics.SkillSystem;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI
{
    public class AttributeView:MonoBehaviour
    {
        public PlayerGameStats.PlayerAttributes Attribute;
        public Button Plus;
        public Button Minus;
        public Text Value;
        
        void Start()
        {
            Plus.onClick.AddListener(() => GameController.GameStats.IncAttribute(Attribute));
            Minus.onClick.AddListener(() => GameController.GameStats.DecAttribute(Attribute));
            UpdateView();
        }
        
        void OnEnable()
        {
            GameController.GameStats.OnAttributeChange += UpdateView;
        }

        void OnDisable()
        {
            GameController.GameStats.OnAttributeChange -= UpdateView;

        }

        void UpdateView()
        {
            Value.text = GameController.GameStats.GetAttribute(Attribute).ToString();
        }
        
        
    }
}