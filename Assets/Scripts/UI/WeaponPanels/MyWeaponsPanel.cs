using System;
using System.Collections.Generic;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking.Interfaces;
using UnityEngine;
using Zenject;

namespace HauntedCity.UI.WeaponPanels
{
    public class MyWeaponsPanel : Panel
    {
        public GameObject WeaponCardPrefab;
        public Transform CardContainer;

        [Inject] private GameController _gameController;
        [Inject] private WeaponLoader _weaponLoader;
        [Inject] private IPlayerStatsManager _playerStatsManager;
        private readonly Dictionary<string, WeaponCard> _weaponCards = new Dictionary<string, WeaponCard>();
        private List<string> _playerWeapons;

        private void Start()
        {
            _playerWeapons = GameController.GameStats.AllowableWeapons;
            if (_weaponLoader == null)
            {
                _weaponLoader = new WeaponLoader();
            }
            UpdateView();
        }

        protected override void OnShow()
        {
            UpdateView();
        }

        protected override void OnHide()
        {
            _playerStatsManager.ChooseWeapons(GameController.GameStats.CurrentWeapons);
        }

        private void OnEnable()
        {
            _gameController.OnPlayerStatsUpdate += UpdateView;
        }

        private void OnDisable()
        {
            _gameController.OnPlayerStatsUpdate -= UpdateView;
        }

        public void UpdateView()
        {
            foreach (var weapon in _playerWeapons)
            {
                if (_weaponCards.ContainsKey(weapon))
                {
                    _weaponCards[weapon].UpdateView();
                }
                else
                {
                    DrawWeaponCard(_weaponLoader.LoadWeapon(weapon));
                }
            }
        }


        private void DrawWeaponCard(Weapon weapon)
        {
            var weaponCard = Instantiate(WeaponCardPrefab);
            weaponCard.transform.SetParent(CardContainer, false);

            var weaponCardComponent = weaponCard.GetComponent<WeaponCard>();
            weaponCardComponent.UpdateView(weapon);
            _weaponCards.Add(weapon.Id, weaponCardComponent);
        }
    }
}