using GameSparks.Api.Responses;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class StoreWeaponCard:WeaponCard
    {
        public Button BuyButton;
        private Weapon _weapon;

        private void Start()
        {     
            BuyButton.onClick.AddListener(Buy);
        }

        public override void UpdateView(Weapon weapon)
        {
            base.UpdateView(weapon);
            _weapon = weapon;
            BuyButton.gameObject.SetActive(
                GameController.GameStats.Money >= weapon.Cost    
            );
        }

        public void Buy()
        {
            GameController.GameStats.TryBuyWeapon(_weapon);
        }
    }
}