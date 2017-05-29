using System;
using UnityEngine;

namespace  HauntedCity.GameMechanics.BattleSystem
{

    public class WeaponLoaderException : Exception
    {
        public WeaponLoaderException(string message):base(message){}
    }

    public class WeaponLoader
    {
        private const string _ROOT_FOLDER = "BattleSystem/Weapons/";

        public static Weapon LoadWeapon(string weaponId)
        {
            var resource = Resources.Load(_ROOT_FOLDER + weaponId) as Weapon;
            if (resource == null)
            {
                throw new WeaponLoaderException("Weapon with id: " + weaponId + " not found");
            }
            return resource;
        }
    }
}