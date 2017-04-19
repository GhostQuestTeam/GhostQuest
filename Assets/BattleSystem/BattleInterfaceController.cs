using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
    public class BattleInterfaceController : MonoBehaviour
    {
        private PlayerBattleController _battleController;


        void Start()
        {
            _battleController = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>().BattleController;
            var weapons = _battleController.BattleStats.Weapons;

            transform.Find("ButtonShoot").GetComponent<Button>().onClick.AddListener(()=> _battleController.TryShoot());

            //TODO Обобщить и не писать как здесь всё в одну строку
            transform.Find("WeaponsPanel/BlasterButton").GetComponent<Button>().onClick.AddListener(() => _battleController.BattleStats.CurrentWeaponId = 1);
            transform.Find("WeaponsPanel/BulletButton").GetComponent<Button>().onClick.AddListener(() => _battleController.BattleStats.CurrentWeaponId = 0);



        }
    }
}