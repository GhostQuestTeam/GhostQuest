using System.Collections.Generic;
using UnityEngine;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking.Interfaces;
using Zenject;

namespace HauntedCity.UI.WeaponPanels
{
    public class WeaponStorePanel:Panel
    {
        public GameObject WeaponCardPrefab;
        public Transform CardContainer;

        [Inject] private IPlayerStatsManager _playerStatsManager;
        [Inject] private WeaponLoader _weaponLoader;
        private readonly Dictionary<string, WeaponCard> _weaponCards = new Dictionary<string, WeaponCard>();
        private List<string> _playerWeapons;
        
        private void Start()
        {
            GameController.GameStats.OnWeaponBuy += OnWeaponBuy;
            _playerWeapons = GameController.GameStats.AllowableWeapons;
            if (_weaponLoader == null)
            {
                _weaponLoader = new WeaponLoader();
            }
            Draw();
        }

        public void Draw()
        {
            foreach (var weapon in _weaponLoader.WeaponList)
            {
                if (!_playerWeapons.Contains(weapon.Id))
                {
                    DrawWeaponCard(weapon);
                }
            } 
        }

        public void UpdateView()
        {
            foreach (var weapon in _weaponLoader.WeaponList)
            {
                if (!_playerWeapons.Contains(weapon.Id) && _weaponCards.ContainsKey(weapon.Id))
                {
                    _weaponCards[weapon.Id].UpdateView(weapon);
                }
            } 
        }
        

        public void OnWeaponBuy(Weapon weapon)
        {
            var toDelete = _weaponCards[weapon.Id];
            _weaponCards.Remove(weapon.Id);
            Destroy(toDelete.gameObject);
            UpdateView();
        }
        
        private void DrawWeaponCard(Weapon weapon)
        {
            var weaponCard = Instantiate(WeaponCardPrefab);
            weaponCard.transform.SetParent( CardContainer, false);
            
            var weaponCardComponent =weaponCard.GetComponent<StoreWeaponCard>();
            weaponCardComponent.PlayerStatsManager = _playerStatsManager;
            weaponCardComponent.UpdateView(weapon);
            _weaponCards.Add(weapon.Id, weaponCardComponent);

        }
    }
}