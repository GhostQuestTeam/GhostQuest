using System;
using System.Collections.Generic;
using System.Linq;
using HauntedCity.GameMechanics.Main;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace HauntedCity.GameMechanics.BattleSystem
{
    public class BattleStateController : MonoBehaviour
    {
        public enum BattleResultType
        {
            WON,
            LOSE,
            STOPED
        }
        public class BattleResult
        {
            public BattleResultType Type { get; private set; }
            public Dictionary<string, int> KilledEnemies { get; private set; }
            public int EarnedExp { get; private set; }

            public BattleResult(BattleResultType type, Dictionary<string, int> killedEnemies, int earnedExp)
            {
                Type = type;
                KilledEnemies = killedEnemies;
                EarnedExp = earnedExp;
            }
        }
        
        public int MaxEnemiesOnScene = 7;
        public float MinDistance = 30f;
        public float MaxDistance = 70f;

        
        private BattleStatsCalculator _battleStatsCalculator;

        private volatile bool _isBattleFinished;
        private int _totalScore;

        private Dictionary<string, int> _allEnemies;
        private Dictionary<string, int> _killedEnemies;
        private HashSet<GameObject> _currentEnemies;
        private PlayerBattleBehavior _player;


        public event Action<BattleResult> OnBattleEnd;

        [Inject]
        public void InitializeDependencies(BattleStatsCalculator battleStatsCalculator)
        {
            _battleStatsCalculator = battleStatsCalculator;
        }
        
        void Awake()
        {
            _currentEnemies = new HashSet<GameObject>();
        }

        void Start()
        {
        }

        public void StartBattle(Dictionary<string, int> enemies)
        {
            
            //Debug.Log("Start battle on scene " + SceneManager.GetActiveScene().name);
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>();
            _player.BattleController.OnDeath += PlayerDeathHandle;

            _totalScore = 0;
            _isBattleFinished = false;
            _allEnemies = enemies;
            
            _ClearEnemies();
            
            _player.Reset(_battleStatsCalculator.CalculateBattleStats(GameController.GameStats));

            SpawnEnemies();
        }

        public void SpawnEnemies()
        {
            while ((_currentEnemies.Count < MaxEnemiesOnScene) && (_allEnemies.Count > 0))
            {
                var randEnemy = _allEnemies.ElementAt(Random.Range(0, _allEnemies.Count)).Key;
                if (_allEnemies[randEnemy] == 0)
                {
                    _allEnemies.Remove(randEnemy);
                    continue;
                }
                _allEnemies[randEnemy]--;
                if (_allEnemies[randEnemy] == 0)
                {
                    _allEnemies.Remove(randEnemy);
                }

                var randomAngle = Random.Range(0.0f, Mathf.PI * 2);
                var randomSize = Random.Range(MinDistance, MaxDistance);
                Vector3 randPosition = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle)) * randomSize;

                var newEnemy = BattleObjectFactory.SpawnEnemy(randEnemy, randPosition);
                newEnemy.GetComponent<EnemyBehavior>().BattleController.OnDeath +=
                    () => EnemyDeathHandle(newEnemy);
                newEnemy.transform.position += _player.transform.position;
                _currentEnemies.Add(newEnemy);
            }
        }

        private void _ClearEnemies()
        {
            foreach (var enemy in _currentEnemies)
            {
                Destroy(enemy);
            }
            _currentEnemies.Clear();
            Debug.Log("Clear Enemies");

        }
        
        private void _EndBattle( BattleResultType battleResultType)
        {
            
            if(OnBattleEnd == null) return;
            BattleResult result = new BattleResult(battleResultType, _killedEnemies, _totalScore);
            OnBattleEnd(result);
        }

        public void EnemyDeathHandle(GameObject enemy)
        {
            _totalScore += enemy.GetComponent<EnemyBehavior>().enemyInfo.Score;
            _currentEnemies.Remove(enemy);
            if (_allEnemies.Count > 0)
            { 
                SpawnEnemies();
            }
            else if ((_currentEnemies.Count == 0) && !_isBattleFinished)
            {
                _EndBattle(BattleResultType.WON);
            }
        }

        public void PlayerDeathHandle()
        {
            _player.BattleController.OnDeath -= PlayerDeathHandle;
            if (!_isBattleFinished)
            {
                _EndBattle(BattleResultType.LOSE);
            }
        }
    }
}