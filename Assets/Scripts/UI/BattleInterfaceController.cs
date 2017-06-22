using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class BattleInterfaceController : MonoBehaviour
    {
        
        
        private PlayerBattleController _battleController;
        public string WeaponPrefabPath = "BattleSystem/UI/WeaponButton";

        [Inject]
        public void InitializeDependencies(PlayerBattleController battleController)
        {
            _battleController = battleController;
        }
        
        
        //private PlayerBattleController _battleController;
        private GameObject _weaponButtonPrefab;
        private Transform _weaponsPanel;

        //Метод Start не срабатывает, а в Awake BattleController ещё не инициализирован
        void OnEnable()
        {

//            _battleController = GameObject.Find("Player").GetComponent<PlayerBattleBehavior>().BattleController;
            transform.Find("ButtonShoot").GetComponent<Button>().onClick
                .AddListener(() => _battleController.TryShoot());

            _weaponButtonPrefab = Resources.Load(WeaponPrefabPath) as GameObject;

            _weaponsPanel = transform.Find("WeaponsPanel");

            _battleController.OnReset += ResetHandle;

        }

        void ResetHandle()
        {
            var battleStats = _battleController.BattleStats;

            _weaponsPanel.Clear();
            for (var i = 0; i < battleStats.Weapons.Length; i++)
            {
                var weapon = battleStats.Weapons[i];
                var button = Instantiate(_weaponButtonPrefab);
                button.transform.Find("WeaponImage").GetComponent<Image>().sprite = weapon.Sprite;

                button.transform.SetParent(_weaponsPanel);
                var tmp = (uint) i;
                button.GetComponent<Button>().onClick.AddListener(() => battleStats.CurrentWeaponId = tmp);
            }
        }
    }
}