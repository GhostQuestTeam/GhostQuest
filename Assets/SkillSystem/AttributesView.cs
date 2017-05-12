using System;
using UnityEngine;
using UnityEngine.UI;

namespace SkillSystem
{
    //TODO Избавиться от копипасты
    public class AttributesView : MonoBehaviour
    {
        #region Interface elements

        private Button _incSurvivability;
        private Button _decSurvivability;
        private Text _survivabilityPoints;

        private Button _incEndurance;
        private Button _decEndurance;
        private Text _endurancePoints;

        private Button _incPower;
        private Button _decPower;
        private Text _powerPoints;

        private Button _okButton;
        private Button _backButton;
        private Text _upgradePoints;

        #endregion

        public event Action OnClose;

        private void _InitElementReferences()
        {
            var head = transform.Find("AttributesWindow/HeadPanel");
            var attributes = transform.Find("AttributesWindow/BodyPanel/AttributesList");

            _incSurvivability = attributes.Find("Survivability/Plus").GetComponent<Button>();
            _decSurvivability = attributes.Find("Survivability/Minus").GetComponent<Button>();
            _survivabilityPoints = attributes.Find("Survivability/Image").GetComponentInChildren<Text>();

            _incEndurance = attributes.Find("Endurance/Plus").GetComponent<Button>();
            _decEndurance = attributes.Find("Endurance/Minus").GetComponent<Button>();
            _endurancePoints = attributes.Find("Endurance/Image/Points").GetComponent<Text>();

            _incPower = attributes.Find("Power/Plus").GetComponent<Button>();
            _decPower = attributes.Find("Power/Minus").GetComponent<Button>();
            _powerPoints = attributes.Find("Power/Image/Points").GetComponent<Text>();

            _okButton = head.Find("OkButton").GetComponent<Button>();
            _backButton = head.Find("BackButton").GetComponent<Button>();
            _upgradePoints = head.Find("UpgradePoints").GetComponent<Text>();
        }

        private void _InitEvents()
        {
            _incSurvivability.onClick.AddListener(
                () => GameController.GameStats.IncAttribute(PlayerGameStats.PlayerAttributes.Survivability));
            _decSurvivability.onClick.AddListener(
                () => GameController.GameStats.DecAttribute(PlayerGameStats.PlayerAttributes.Survivability));

            _incEndurance.onClick.AddListener(
                () => GameController.GameStats.IncAttribute(PlayerGameStats.PlayerAttributes.Endurance));
            _decEndurance.onClick.AddListener(
                () => GameController.GameStats.DecAttribute(PlayerGameStats.PlayerAttributes.Endurance));

            _incPower.onClick.AddListener(
                () => GameController.GameStats.IncAttribute(PlayerGameStats.PlayerAttributes.Power));
            _decPower.onClick.AddListener(
                () => GameController.GameStats.DecAttribute(PlayerGameStats.PlayerAttributes.Power));

            _okButton.onClick.AddListener(() =>
            {
                GameController.GameStats.ConfirmUpgrades();
                Close();
            });

            _backButton.onClick.AddListener(Close);

            GameController.GameStats.OnAttributeChange += _UpdateView;
        }

        private void _UpdateView()
        {
            _survivabilityPoints.text = GameController.GameStats.Survivability.ToString();
            _endurancePoints.text = GameController.GameStats.Endurance.ToString();
            _powerPoints.text = GameController.GameStats.Power.ToString();
            _upgradePoints.text = "Upgrade points: " + GameController.GameStats.UpgradePoints;
        }

        void Start()
        {
            _InitElementReferences();
            _InitEvents();
            _UpdateView();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            if(OnClose != null){
                OnClose();
            }
        }

        private void OnDestroy()
        {
            GameController.GameStats.OnAttributeChange -= _UpdateView;
        }
    }
}