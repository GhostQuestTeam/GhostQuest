using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HauntedCity.GameMechanics.BattleSystem
{
    [RequireComponent(typeof(Animator))]
    public class EnemyBehavior : MonoBehaviour
    {
        public enum AttackType
        {
            Melee,
            Range,
            Kamikaze
        }
        
        private WaitForSecondsRealtime _microWait;
        private GameObject _followee;
        private Animator _animator;

        public AttackType attackType = AttackType.Kamikaze;
        public EnemyBattleController BattleController { get; private set; }
        public EnemyBattleStats BattleStats;
        public float DeathDelay = 0f;
        public float AttackRange = 1f;
        public float AttackCooldown = 1f;
        public int Score = 100;
      


        // Use this for initialization
        void Awake()
        {
            _microWait = new WaitForSecondsRealtime(0.2f);
            _animator = GetComponent<Animator>();
            _followee = GameObject.FindWithTag("Player");
            BattleStats.ResetHealth();
            BattleController = new EnemyBattleController(BattleStats);
            BattleController.OnDeath += () => StartCoroutine(Kill(DeathDelay));
            BattleController.OnDamage += (damage) => _animator.SetTrigger("Hit");
        }

        private void Start()
        {
            StartCoroutine(AttackLoop());
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(_followee.transform);
            var distance = Vector3.Distance(transform.position, _followee.transform.position);
            if (distance > AttackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, _followee.transform.position,
                    BattleStats.Velocity);
            }
        }

        IEnumerator AttackLoop()
        {
            while (true)
            {
                var distance = Vector3.Distance(transform.position, _followee.transform.position);
                if (distance > AttackRange)
                {
                    yield return _microWait;
                    continue;
                }
                var player = _followee.GetComponent<PlayerBattleBehavior>();
                _animator.SetTrigger("Attack");
                switch (attackType)
                {
                    case AttackType.Melee:                            
                        player.BattleController.TakeDamage(BattleStats.Damage);
                        break;
                    case AttackType.Kamikaze:
                        player.BattleController.TakeDamage(BattleStats.Damage);
                        BattleController.Kill();
                        break;
                    case AttackType.Range:
                        //TODO
                        break;
                            
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                    
                yield return new WaitForSeconds(AttackCooldown);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            var shell = other.gameObject.GetComponent<ShellBehavior>();
            if (shell != null)
            {
                BattleController.TakeDamage(shell.Weapon.Damage);
            }
//
//            if (other.gameObject.CompareTag("Player"))
//            {
//                var player = other.gameObject.GetComponent<PlayerBattleBehavior>();
//                player.BattleController.TakeDamage(BattleStats.Damage);
//                BattleController.Kill();
//            }
        }

        public IEnumerator Kill(float delay)
        {
            BattleStats.Velocity = 0;
            _animator.SetTrigger("Die");
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}