using System;
using System.Collections;
using System.Collections.Generic;
using HauntedCity.UI;
using UnityEngine;
using HauntedCity.Utils;
using Zenject;
using UnityEngine.SceneManagement;

namespace HauntedCity.GameMechanics.BattleSystem
{
    [RequireComponent(typeof(Animator))]
    public class EnemyBehavior : MonoBehaviour
    {
        
        
        private WaitForSecondsRealtime _microWait;
        private GameObject _followee;
        private Animator _animator;

        public Bar HealthBar;
        
        public EnemyBattleController BattleController { get; private set; }
        private EnemyBattleStats _battleStats;
        public EnemyInfo enemyInfo;
        
        [Inject] SceneAgregator  _sceneAgregator;

        // Use this for initialization
        void Awake()
        {
            
            _battleStats = new EnemyBattleStats(
                 new Solidity((uint)enemyInfo.MaxHealth, enemyInfo.Defence),
                enemyInfo.Velocity,
                enemyInfo.Damage
             );
            _microWait = new WaitForSecondsRealtime(0.2f);
            _animator = GetComponent<Animator>();
            _followee = GameObject.FindWithTag("Player");
            _battleStats.ResetHealth();
            BattleController = new EnemyBattleController(_battleStats);
            BattleController.OnDeath += () => StartCoroutine(Kill(enemyInfo.DeathDelay));
            //BattleController.OnHealthChange += (damage) => _animator.SetTrigger("Hit");
        }

        private void Start()
        {
            HealthBar.Value = HealthBar.Max = enemyInfo.MaxHealth;
            StartCoroutine(AttackLoop());
        }

        
        // Update is called once per frame
        void FixedUpdate()
        {
            transform.LookAt(_followee.transform);
            var distance = Vector3.Distance(transform.position, _followee.transform.position);
            if (distance > enemyInfo.AttackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, _followee.transform.position,
                    _battleStats.Velocity);
            }
            if (SceneManager.GetActiveScene().name != "battle" )
            {
                Destroy(gameObject);
            }
        }

        IEnumerator AttackLoop()
        {
            while (true)
            {
                var distance = Vector3.Distance(transform.position, _followee.transform.position);
                if (distance > enemyInfo.AttackRange)
                {
                    yield return _microWait;
                    continue;
                }
                var player = _followee.GetComponent<PlayerBattleBehavior>();
                _animator.SetTrigger("Attack");
                switch (enemyInfo.attackType)
                {
                    case AttackType.Melee:                            
                        player.BattleController.TakeDamage(_battleStats.Damage);
                        break;
                    case AttackType.Kamikaze:
                        player.BattleController.TakeDamage(_battleStats.Damage);
                        BattleController.Kill();
                        break;
                    case AttackType.Range:
                        //TODO
                        break;
                            
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                    
                yield return new WaitForSecondsRealtime(enemyInfo.AttackCooldown);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            var shell = other.gameObject.GetComponent<ShellBehavior>();
            if (shell != null)
            {
                BattleController.TakeDamage(shell.Weapon.Damage);
                HealthBar.Value = BattleController.BattleStats.CurrentHealth;
                _animator.SetTrigger("Hit");
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    -1*_followee.transform.position,
                    shell.Weapon.Force);

            }
        }

        public IEnumerator Kill(float delay)
        {
            _battleStats.Velocity = 0;
            _animator.SetTrigger("Die");
            yield return new WaitForSeconds(delay);
            var randValue = UnityEngine.Random.Range(0f, 1f);
            if (randValue <= enemyInfo.BonusDropProb)
            {
               var bonus = BattleObjectFactory.RandomBonus();
               bonus.transform.position = transform.position;
            }
            
            Destroy(gameObject);
            
        }
    }
}