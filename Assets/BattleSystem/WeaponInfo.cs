using System;
using UnityEngine;

namespace BattleSystem
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

    public class WeaponInfo
    {
        public string Id { get; }
        public int ShootCost { get; set; }
        public double Cooldown { get; set; }
        public ShellInfo Shell { get; }

        public WeaponInfo(string id, int shootCost, double cooldown)
        {
            Id = id;
            ShootCost = shootCost;
            Cooldown = cooldown;
        }

        public WeaponInfo(string id, int shootCost, double cooldown, ShellInfo shell) : this(id, shootCost, cooldown)
        {
            Shell = shell;
        }

        public WeaponInfo(string id, int shootCost, double cooldown, int damage, int velocity, ShellEffect effect = null) :
            this(id, shootCost, cooldown)
        {
            Shell = new ShellInfo(damage, velocity ,effect);
        }
    }
}