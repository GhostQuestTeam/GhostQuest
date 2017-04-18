using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class LoadShellException:Exception
    {
        public LoadShellException( string id):base("Couldn't load shell with id: " + id){}
    }

    public class ShellFactory
    {

        private const string _FOLDER = "BattleSystem/Shells/";
        private Dictionary<string, GameObject> _cache;

        public GameObject CreateShell(WeaponInfo weaponInfo)
        {
            string id = weaponInfo.Id;
            if (!_cache.ContainsKey(id))
            {
                var prefab = Resources.Load(_FOLDER + id) as GameObject;
                if (prefab == null)
                {
                    throw  new LoadShellException(id);
                }
                _cache.Add(id, prefab);
            }
            return GameObject.Instantiate(_cache[id]);

        }

        public ShellFactory()
        {
            _cache = new Dictionary<string, GameObject>();
        }
    }
}