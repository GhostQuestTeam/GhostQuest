using UnityEngine;
using HauntedCity.GameMechanics.BattleSystem;
using Zenject;

namespace HauntedCity.UI
{
    public class WeaponStorePanel:MonoBehaviour
    {
        public GameObject WeaponCardPrefab;
        public Transform CardContainer;
        
        [Inject] private WeaponLoader _weaponLoader;

        private void Start()
        {
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
                    DrawWeaponCard(weapon);
            } 
        }

        public void DrawWeaponCard(Weapon weapon)
        {
            var weaponCard = Instantiate(WeaponCardPrefab);
            weaponCard.transform.SetParent( CardContainer, false);
            
            weaponCard.GetComponent<WeaponCard>().UpdateView(weapon);
        }
    }
}