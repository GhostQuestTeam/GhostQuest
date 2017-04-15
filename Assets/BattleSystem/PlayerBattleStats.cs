using UnityEditor;
using UnityEngine;
using Utils;

namespace BattleSystem
{
    public class PlayerBattleStats
    {
        private BoundedInt _energy;

        public Solidity Solidity { get; set; }
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

        public uint MaxHealth
        {
            get { return Solidity.MaxHealth; }
            set { Solidity.MaxHealth = value; }
        }

        public int CurrentHealth => Solidity.CurrentHealth;

        public PlayerBattleStats(Solidity solidity, int maxEnergy, int energyRegen)
        {
            Solidity = solidity;
            _energy = new BoundedInt(maxEnergy, 0, maxEnergy);
            EnergyRegen = energyRegen;
        }

    }
}