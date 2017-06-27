using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Utils.Extensions;
using UnityEngine.UI;

namespace HauntedCity.UI
{
    public class MyWeaponCard:WeaponCard
    {
        public Toggle UseWeapon;
        private void Start()
        {
            UseWeapon.onValueChanged.AddListener(OnToggleValueChanged);
        }

        public override void UpdateView()
        {
           base.UpdateView();
           UseWeapon.SetState( GameController.GameStats.CurrentWeapons.Contains(_weapon.Id) );
        }

        //TODO ограничить количество используемых оружий
        public void OnToggleValueChanged(bool enabled)
        {
            if (enabled)
            {
                GameController.GameStats.CurrentWeapons.Add(_weapon.Id);
            }
            else
            {
                GameController.GameStats.CurrentWeapons.Remove(_weapon.Id);
            }
        }
    }
}