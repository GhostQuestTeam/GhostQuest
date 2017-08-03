using GameSparks.Api.Responses;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.WeaponPanels
{
    public class StoreWeaponCard:WeaponCard
    {
        public Button BuyButton;
        public Text Price;

        public IPlayerStatsManager PlayerStatsManager { get; set; }
        
        private void Start()
        {     
            BuyButton.onClick.AddListener(Buy);
        }

        public override void UpdateView()
        {
            base.UpdateView();
            Price.text = _weapon.Cost.ToString();
            BuyButton.gameObject.SetActive(
                GameController.GameStats.Money >= _weapon.Cost    
            );
        }

        public void Buy()
        {
            GameController.GameStats.TryBuyWeapon(_weapon);
            Debug.Log("Try buy weapon with ID: " + _weapon.Id);
            PlayerStatsManager.BuyWeapon(_weapon.Id);
        }
    }
}