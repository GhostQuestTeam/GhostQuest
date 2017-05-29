using HauntedCity.GameMechanics.BattleSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class BattleInterfaceController : MonoBehaviour
    {
        
        
        private PlayerBattleBehavior _player;
        public string WeaponPrefabPath = "BattleSystem/UI/WeaponButton";

        [Inject]
        public void InitializeDependencies(PlayerBattleBehavior player)
        {
            _player = player;
        }
        
        //private PlayerBattleController _battleController;
        private GameObject _weaponButtonPrefab;
        private Transform _weaponsPanel;


        void Start()
        {
            //_battleController = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>().BattleController;

            transform.Find("ButtonShoot").GetComponent<Button>().onClick
                .AddListener(() => _player.BattleController.TryShoot());

            _weaponButtonPrefab = Resources.Load(WeaponPrefabPath) as GameObject;

            _weaponsPanel = transform.Find("WeaponsPanel");

            _player.BattleController.OnReset += ResetHandle;

        }

        void ResetHandle()
        {
            var battleStats = _player.BattleController.BattleStats;

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