using System;
using UnityEngine;
using Utils;

namespace BattleSystem
{
    [Serializable]
    public class BattleStats
    {
        [SerializeField]
        public Solidity Solidity;

        public uint MaxHealth
        {
            get { return Solidity.MaxHealth; }
            set { Solidity.MaxHealth = value; }
        }

        public int CurrentHealth
        {
            get { return Solidity.CurrentHealth; }

        }

        public void ResetHealth()
        {
            Solidity.ResetHealth();
        }

        public BattleStats(Solidity solidity)
        {
            Solidity = solidity;
        }

    }

    public class PlayerBattleStats:BattleStats
    {
        private BoundedInt _energy;

        public Weapon[] Weapons { get; private set; }
        public uint CurrentWeaponId { get; set; }
        public Weapon CurrentWeapon
        {
            get { return Weapons[CurrentWeaponId]; }
        }

        public int EnergyRegen { get; set; }
        public int MaxEnergy
        {
            get { return _energy.Max; }
            set { _energy.Max = value; }
        }

        public int CurrentEnergy
        {
            get { return _energy.Val; }
            set { _energy.Val = value; }
        }


        public PlayerBattleStats(Solidity solidity, int maxEnergy, int energyRegen, Weapon[] weapons): base(solidity)
        {
            _energy = new BoundedInt(maxEnergy, 0, maxEnergy);
            EnergyRegen = energyRegen;
            Weapons = weapons;
            CurrentWeaponId = 0;
        }

    }

    [Serializable]
    public class EnemyBattleStats:BattleStats
    {
        public float Velocity;

        public int Damage;

        public EnemyBattleStats(Solidity solidity, float velocity,int damage) : base(solidity)
        {
            Velocity = velocity;
            Damage = damage;
        }
    }
}