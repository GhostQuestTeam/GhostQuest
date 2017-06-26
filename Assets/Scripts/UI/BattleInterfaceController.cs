using System;
using System.Collections;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class BattleInterfaceController : MonoBehaviour
    {
        private const float TIME_DELTA = 0.1f;
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
        private Image[] _weaponCooldowns;

        //Метод Start не срабатывает, а в Awake BattleController ещё не инициализирован
        void OnEnable()
        {
//            _battleController = GameObject.Find("Player").GetComponent<PlayerBattleBehavior>().BattleController;
            transform.Find("ButtonShoot").GetComponent<Button>().onClick
                .AddListener(() =>
                    {
                        var shootSuccess = _battleController.TryShoot();
                        if (shootSuccess)
                        {
                            StartCoroutine(CooldownVisualise(Array.IndexOf(_battleController.BattleStats.Weapons,
                                _battleController.BattleStats.CurrentWeapon)));
                        }
                    }
                );

            _weaponButtonPrefab = Resources.Load(WeaponPrefabPath) as GameObject;

            _weaponsPanel = transform.Find("WeaponPanelContainer/WeaponsPanel");

            _battleController.OnReset += ResetHandle;
        }

        IEnumerator CooldownVisualise(int weaponIndex)
        {
            var cooldown = _battleController.BattleStats.Weapons[weaponIndex].Cooldown;
            var progress = 0f;

            while (progress < cooldown)
            {
                progress += TIME_DELTA;
                yield return new WaitForSecondsRealtime(TIME_DELTA);
                _weaponCooldowns[weaponIndex].fillAmount = progress / cooldown;
            }
            _weaponCooldowns[weaponIndex].fillAmount = 0f;
        }

        void ResetHandle()
        {
            var battleStats = _battleController.BattleStats;

            _weaponsPanel.Clear();
            _weaponCooldowns = new Image[battleStats.Weapons.Length];
            for (var i = 0; i < battleStats.Weapons.Length; i++)
            {
                var weapon = battleStats.Weapons[i];
                var button = Instantiate(_weaponButtonPrefab);
                button.transform.Find("WeaponImage").GetComponent<Image>().sprite = weapon.Sprite;

                button.transform.SetParent(_weaponsPanel, false);
                var tmp = (uint) i;
                
                button.GetComponent<Button>().onClick.AddListener(() => { battleStats.CurrentWeaponId = tmp; });

                _weaponCooldowns[i] = button.transform.Find("Cooldown").GetComponent<Image>();
                _weaponCooldowns[i].fillAmount = 0f;
            }
            
        }
    }
}