using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleSystem
{
    public class BattleStateController:MonoBehaviour
    {
        public int   MaxEnemiesOnScene = 7;
        public float MinDistance = 30f;
        public float MaxDistance = 70f;


        private const string _ENEMIES_PREFABS_FOLDER = "BattleSystem/Enemies/";
        private bool _isBattleFinished;
        private int _totalScore;

        private Dictionary<string, int> _allEnemies;
        private HashSet<GameObject> _currentEnemies;
        private PlayerBattleBehavior _player;


        public event Action<int> OnWon;
        public event Action OnLose;

        void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            _currentEnemies = new HashSet<GameObject>();
        }

        void Start()
        {
        }

        public void StartBattle(Dictionary<string, int> enemies)
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>();
            _player.BattleController.OnDeath += PlayerDeathHandle;

            _totalScore = 0;
            _isBattleFinished = false;
            _allEnemies = enemies;
            foreach (var enemy in _currentEnemies)
            {
               Destroy(enemy);
            }
            _currentEnemies.Clear();
            _player.Reset();

            SpawnEnemies();
        }

        public void SpawnEnemies()
        {
            while ((_currentEnemies.Count < MaxEnemiesOnScene) && (_allEnemies.Count > 0))
            {
                var randEnemy = _allEnemies.ElementAt(Random.Range(0, _allEnemies.Count)).Key;
                _allEnemies[randEnemy]--;
                if (_allEnemies[randEnemy] == 0)
                {
                    _allEnemies.Remove(randEnemy);
                }

                var randomAngle = Random.Range(0.0f,Mathf.PI*2);
                var randomSize = Random.Range(MinDistance, MaxDistance);
                Vector3 randPosition = new Vector3(Mathf.Sin(randomAngle),0,Mathf.Cos(randomAngle)) * randomSize;

                var newEnemy = BattleObjectFactory.SpawnEnemy(randEnemy, randPosition);
                newEnemy.GetComponent<EnemyBehavior>().BattleController.OnDeath +=
                    () =>EnemyDeathHandle(newEnemy) ;
                newEnemy.transform.position += _player.transform.position;
                _currentEnemies.Add(newEnemy);
            }
        }

        public void EnemyDeathHandle(GameObject enemy)
        {
            _totalScore += enemy.GetComponent<EnemyBehavior>().Score;
            _currentEnemies.Remove(enemy);
            if (_allEnemies.Count > 0 )
            {
                SpawnEnemies();
            } else if((_currentEnemies.Count == 0) && !_isBattleFinished )
            {
                Debug.Log("You win! Score: " + _totalScore);
                if (OnWon != null)
                {
                    OnWon(_totalScore);
                }
                _isBattleFinished = true;
            }

        }

        public void PlayerDeathHandle()
        {
            _player.BattleController.OnDeath -= PlayerDeathHandle;
            if (!_isBattleFinished)
            {
                _isBattleFinished = true;
                Debug.Log("You lose");
                if (OnLose != null)
                {
                    OnLose();
                }
            }
        }

    }
}