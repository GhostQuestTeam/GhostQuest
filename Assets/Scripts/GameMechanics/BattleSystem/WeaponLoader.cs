using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HauntedCity.GameMechanics.BattleSystem
{
    public class WeaponLoaderException : Exception
    {
        public WeaponLoaderException(string message) : base(message)
        {
        }
    }

    public class WeaponLoader
    {
        public const string DEFAULT_ROOT_FOLDER = "BattleSystem/Weapons/";
        private string _rootFolder;
        private Dictionary<string, Weapon> _weapons ;

        public Weapon[] WeaponList
        {
            get { return _weapons.Values.ToArray(); }
        }
        public Weapon LoadWeapon(string weaponId)
        {
            if (_weapons.ContainsKey(weaponId))
            {
                return _weapons[weaponId];
            }
            var resource = Resources.Load(_rootFolder + weaponId) as Weapon;
            if (resource == null)
            {
                throw new WeaponLoaderException("Weapon with id: " + weaponId + " not found");
            }
            _weapons.Add(resource.Id, resource);
            return resource;
        }

        public WeaponLoader(string rootFolder= DEFAULT_ROOT_FOLDER)
        {
            _rootFolder = rootFolder;
            _weapons = Resources
                .LoadAll(_rootFolder)
                .Select(w => w as Weapon)
                .ToDictionary(w => w.Id, w => w);
            
        }
    }
}