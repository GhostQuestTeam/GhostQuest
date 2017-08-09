using HauntedCity.GameMechanics.BattleSystem;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI.WeaponPanels
{
    public class WeaponCard:MonoBehaviour
    {
        public Image WeaponImage;
        public Text Title;
        public Text Damage;
        public Text EnergyCost;
        public Text Cooldown;

        protected Weapon _weapon;
        
        public virtual void UpdateView(Weapon weapon)
        {
            _weapon = weapon;
            UpdateView();
        }
        
        public virtual void UpdateView()
        {
            WeaponImage.sprite = _weapon.Sprite;
            Title.text = _weapon.Title;
            Damage.text = "Damage: " + _weapon.Damage;
            EnergyCost.text = "Energy cost: " + _weapon.ShootCost;
            Cooldown.text = "Cooldown: " + _weapon.Cooldown + " sec";
        }
    }
}