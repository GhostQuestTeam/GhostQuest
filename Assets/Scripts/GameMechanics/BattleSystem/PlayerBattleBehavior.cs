using System;
using System.Collections;
using System.Collections.Generic;
using HauntedCity.GameMechanics.Main;
using UnityEngine;
using Zenject;

namespace HauntedCity.GameMechanics.BattleSystem
{
    public class PlayerBattleBehavior : MonoBehaviour, IShooter, IWeaponSpeedController
    {
        
        public PlayerBattleController BattleController;

        private int _direction = -1;

        public string[] WeaponIDs;


        private Dictionary<string, bool> _blockedWeapons;

        void Awake()
        {
            _blockedWeapons = new Dictionary<string, bool>();
            PlayerBattleStats battleStats = null;
            //var battleStats = _battleStatsCalculator.CalculateBattleStats(GameController.GameStats);

            BattleController = new PlayerBattleController(null, this, this);
        }
        
        
        void Start()
        {
           
        }

        public void Reset(PlayerBattleStats battleStats)
        {
            _blockedWeapons.Clear();
            foreach (var weapon in battleStats.Weapons)
            {
                _blockedWeapons.Add(weapon.Id, false);
            }
            BattleController.BattleStats = battleStats;
            BattleController.Reset();
            StartCoroutine(Regenerate());
        }

        void Update()
        {
        }



        public void Shoot(Weapon weapon)
        {
            
            GameObject shell = Instantiate(weapon.Prefab);
            var cameraForward = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().transform.forward;
            var startPosition = transform.position + cameraForward * 1;
            shell.GetComponent<ShellBehavior>().Weapon = weapon;
            shell.GetComponent<ShellBehavior>().Launch(startPosition, cameraForward);
        }

        public IEnumerator Regenerate()
        {
            while (true)
            {
                BattleController.Regenerate();
                yield return new WaitForSeconds(1f);
            }
        }

        public void OnShootClick()
        {
            BattleController.TryShoot();
        }


        private IEnumerator _blockWeaponCoroutine(float sec, string weapontId)
        {
            _blockedWeapons[weapontId] = true;
            yield return new WaitForSeconds(sec);
            _blockedWeapons[weapontId] = false;
        }

        public bool CanShoot(Weapon weapon)
        {
            return !_blockedWeapons[weapon.Id];
        }

        public void BlockWeapon(Weapon weapon, float duration)
        {
            StartCoroutine(_blockWeaponCoroutine(duration, weapon.Id));
        }
    }
}