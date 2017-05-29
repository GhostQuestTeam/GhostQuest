using System;
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
        
        public Weapon LoadWeapon(string weaponId)
        {
            var resource = Resources.Load(_rootFolder + weaponId) as Weapon;
            if (resource == null)
            {
                throw new WeaponLoaderException("Weapon with id: " + weaponId + " not found");
            }
            return resource;
        }

        public WeaponLoader(string rootFolder= DEFAULT_ROOT_FOLDER)
        {
            _rootFolder = rootFolder;
        }
    }
}