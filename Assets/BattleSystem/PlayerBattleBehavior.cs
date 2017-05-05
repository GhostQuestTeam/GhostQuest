using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

namespace BattleSystem
{
    public class PlayerBattleBehavior : MonoBehaviour, IShooter, IWeaponSpeedController
    {
        public PlayerBattleController BattleController; // { get; private set; }

        private int _direction = -1;

        public string[] WeaponIDs;

        private Dictionary<string, bool> _blockedWeapons;

        void Awake()
        {
            var solidity = new Solidity(100, 0);
            var weapons = new WeaponInfo[WeaponIDs.Length];
            _blockedWeapons = new Dictionary<string, bool>();
            for (var i = 0; i < WeaponIDs.Length; i++)
            {
                weapons[i] = WeaponLoader.LoadWeapon(WeaponIDs[i]);
                _blockedWeapons.Add(weapons[i].Id, false);
            }

            var battleStats = new PlayerBattleStats(solidity, 100, 1, weapons);
            BattleController = new PlayerBattleController(battleStats, this, this);

            StartCoroutine(Regenerate());
            StartCoroutine(Oscillate());
        }

        void Update()
        {
        }

        // Костыль, нужный, чтобы своевременно обрабатывалось столкновение, т.к
        // OnCollisionEnter вызывается только, когда 2 объекта в движении
        public IEnumerator Oscillate()
        {
            while (true)
            {
                transform.position += _direction * Vector3.down * 0.0000001f;
                _direction *= -1;
                yield return new WaitForSeconds(0.01f);
            }
        }


        public void Shoot(WeaponInfo weapon)
        {
            GameObject shell = BattleObjectFactory.CreateShell(weapon);
            var cameraForward = GetComponentInChildren<Camera>().transform.forward;
            var startPosition = transform.position + cameraForward * 1;
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

        public bool CanShoot(WeaponInfo weapon)
        {
            return !_blockedWeapons[weapon.Id];
        }

        public void BlockWeapon(WeaponInfo weapon, float duration)
        {
            StartCoroutine(_blockWeaponCoroutine(duration, weapon.Id));
        }
    }
}