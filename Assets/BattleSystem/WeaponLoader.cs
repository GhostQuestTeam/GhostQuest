using System;
using UnityEngine;

namespace BattleSystem
{

    public class WeaponLoaderException : Exception
    {
        public WeaponLoaderException(string message):base(message){}
    }

    public class WeaponLoader
    {
        private const string _ROOT_FOLDER = "BattleSystem/Weapons/";

        public static WeaponInfo LoadWeapon(string weaponId)
        {
            var resource = Resources.Load<TextAsset>(_ROOT_FOLDER + weaponId);
            if (resource == null)
            {
                throw new WeaponLoaderException("Weapon with id: " + weaponId + " not found");
            }
            var json = resource.text;
            var result = JsonUtility.FromJson<WeaponInfo>(json);
            return result;
        }
    }
}