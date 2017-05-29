using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
    public class BattleInterfaceController : MonoBehaviour
    {
        public string WeaponPrefabPath = "BattleSystem/UI/WeaponButton";

        private PlayerBattleController _battleController;
        private GameObject _weaponButtonPrefab;
        private Transform _weaponsPanel;


        void Start()
        {
            _battleController = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>().BattleController;
            var weapons = _battleController.BattleStats.Weapons;
            var battleStats = _battleController.BattleStats;

            transform.Find("ButtonShoot").GetComponent<Button>().onClick.AddListener(()=> _battleController.TryShoot());

            _weaponButtonPrefab = Resources.Load(WeaponPrefabPath) as GameObject;

            _weaponsPanel = transform.Find("WeaponsPanel");

            for (var i = 0; i < battleStats.Weapons.Length; i++)
            {
                var weapon = battleStats.Weapons[i];
                var button = Instantiate(_weaponButtonPrefab);
                //var sprite = Resources.Load<Sprite>(_SPRITES_FOLDER + weapon.Id);
                button.transform.Find("WeaponImage").GetComponent<Image>().sprite = weapon.Sprite;

                button.transform.SetParent(_weaponsPanel);
                var tmp = (uint)i;
                button.GetComponent<Button>().onClick.AddListener(() => battleStats.CurrentWeaponId = tmp);
            }



        }
    }
}