using GameSparks.Api.Responses;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.WeaponPanels
{
    public class StoreWeaponCard:WeaponCard
    {
        public Button BuyButton;
        public Text Price;
        
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
        }
    }
}