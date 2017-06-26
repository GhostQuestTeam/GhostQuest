using HauntedCity.GameMechanics.BattleSystem;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI
{
    public class WeaponCard:MonoBehaviour
    {
        public Image WeaponImage;
        public Text Title;
        public Text Damage;
        public Text EnergyCost;
        public Text Cooldown;
        public Text Price;

        public void UpdateView(Weapon weapon)
        {
            WeaponImage.sprite = weapon.Sprite;
            Title.text = weapon.Title;
            Damage.text = "Damage: " + weapon.Damage;
            EnergyCost.text = "Energy cost: " + weapon.ShootCost;
            Cooldown.text = "Cooldown: " + weapon.Cooldown + " sec";
            Price.text = weapon.Cost.ToString();
        }
    }
}