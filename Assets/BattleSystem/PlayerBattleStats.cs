using UnityEditor;
using UnityEngine;
using Utils;

namespace BattleSystem
{
    public class PlayerBattleStats:BattleStats
    {
        private BoundedInt _energy;

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


        public PlayerBattleStats(Solidity solidity, int maxEnergy, int energyRegen): base(solidity)
        {
            _energy = new BoundedInt(maxEnergy, 0, maxEnergy);
            EnergyRegen = energyRegen;
        }

    }
}