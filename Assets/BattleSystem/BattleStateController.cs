using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleSystem
{
    public class BattleStateController:MonoBehaviour
    {
        public int   MaxEnemiesOnScene = 7;
        public float MinDistance = 30f;
        public float MaxDistance = 70f;

        private const string _ENEMIES_PREFABS_FOLDER = "BattleSystem/Enemies/";

        private Dictionary<string, int> _allEnemies;
        private HashSet<GameObject> _currentEnemies;


        void Awake()
        {
            _currentEnemies = new HashSet<GameObject>();
            //TODO Вынести вызов этого метода в отдельный класс
            StartBattle(new Dictionary<string, int>(){{"skull_ghost", 5}});
        }

        public void StartBattle(Dictionary<string, int> enemies)
        {
            _currentEnemies.Clear();
            _allEnemies = enemies;
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

                var newEnemy = SpawnEnemy(randEnemy, randPosition);
                _currentEnemies.Add(newEnemy);
            }
        }

        public GameObject SpawnEnemy(string enemyId, Vector3 position)
        {
            //TODO Не загружать префаб каждый раз
            var enemyPrefab = Resources.Load(_ENEMIES_PREFABS_FOLDER + enemyId) as GameObject;

            var enemy = Instantiate(enemyPrefab);
            enemy.transform.position = position;
            return enemy;
        }

    }
}