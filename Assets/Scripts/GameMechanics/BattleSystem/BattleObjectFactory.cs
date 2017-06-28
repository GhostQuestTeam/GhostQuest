using System;
using System.Collections.Generic;
using UnityEngine;

namespace HauntedCity.GameMechanics.BattleSystem
{
    public class LoadBattleObjectException : Exception
    {
        public LoadBattleObjectException(string id) : base("Couldn't load battle object with id: " + id)
        {
        }
    }

    public class BattleObjectLoader
    {
        private const string _BASE_FOLDER = "BattleSystem/";
        private string _objectFolder;
        private Dictionary<string, GameObject> _cache;

        public GameObject Load(string id)
        {
            if (!_cache.ContainsKey(id))
            {
                var prefab = Resources.Load(_BASE_FOLDER + _objectFolder + id) as GameObject;
                if (prefab == null)
                {
                    throw new LoadBattleObjectException(id);
                }
                _cache.Add(id, prefab);
            }
            return GameObject.Instantiate(_cache[id]);
        }

        public BattleObjectLoader(string objectFolder)
        {
            _cache = new Dictionary<string, GameObject>();
            _objectFolder = objectFolder;
        }
    }

    public static class BattleObjectFactory
    {
        private static BattleObjectLoader _shellLoader = new BattleObjectLoader("Shells/");
        private static BattleObjectLoader _enemyLoader = new BattleObjectLoader("Enemies/");
        private static BattleObjectLoader _bonusLoader = new BattleObjectLoader("Bonuses/");

        private static readonly string[] Bonuses = new[]{"HealthBonus", "EnergyBonus"}; 

        public static GameObject CreateShell(Weapon weaponInfo)
        {
            return _shellLoader.Load(weaponInfo.Id);
        }

        public static GameObject SpawnEnemy(string enemyId, Vector3 position)
        {
            var enemy = _enemyLoader.Load(enemyId + "/" + enemyId);//временно
            enemy.transform.position = position;
            return enemy;
        }

        public static GameObject RandomBonus()
        {
            int index = UnityEngine.Random.Range(0, Bonuses.Length);
            return _bonusLoader.Load(Bonuses[index]);
        }
    }
}