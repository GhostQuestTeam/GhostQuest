using System;
using UnityEngine;

namespace  HauntedCity.GameMechanics.BattleSystem
{
    public delegate void ShellEffect(BattleStats stats);

    [Serializable]
    public class ShellInfo
    {

        public ShellEffect Effect;

        //Не свойства, чтобы были видны из инспектора Unity
        public int Damage;
        public float Velocity;

        public ShellInfo(int damage, int velocity, ShellEffect effect = null)
        {
            Damage = damage;
            Velocity = velocity;
        }
    }

    [Serializable]
    public class WeaponInfo
    {
        public string Id;//{ get; }
        public int ShootCost;// { get; set; }
        public float Cooldown;// { get; set; }

        public WeaponInfo(string id, int shootCost, float cooldown)
        {
            Id = id;
            ShootCost = shootCost;
            Cooldown = cooldown;
        }

    }
}