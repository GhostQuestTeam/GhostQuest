using HauntedCity.GameMechanics.BattleSystem;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI
{
    public class WeaponChooseController : MonoBehaviour
    {
        public const string DEFAULT_BUTTON_PREFAB = "DialoguesUI/DialogueAnswer";
        public string ButtonPrefabPath = DEFAULT_BUTTON_PREFAB;

        private GameObject _buttonPrefab;
        private PlayerBattleController _battleController;

        void Start()
        {
            _buttonPrefab = Resources.Load(ButtonPrefabPath) as GameObject;
            _battleController = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>().BattleController;
            var weapons = _battleController.BattleStats.Weapons;
            for (int i = 0; i < weapons.Length; i++)
            {
                var button = Instantiate(_buttonPrefab);
                button.GetComponentInChildren<Text>().text = i.ToString();
                button.transform.SetParent(transform);
                var tmp = i;
                button.GetComponent<Button>()
                    .onClick.AddListener(() => _battleController.BattleStats.CurrentWeaponId = (uint) tmp);
            }
        }
    }
}