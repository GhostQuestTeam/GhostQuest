using Utils;

namespace BattleSystem
{
    public class BattleStats
    {
        public Solidity Solidity { get; set; }

        public uint MaxHealth
        {
            get { return Solidity.MaxHealth; }
            set { Solidity.MaxHealth = value; }
        }

        public int CurrentHealth => Solidity.CurrentHealth;

        public BattleStats(Solidity solidity)
        {
            Solidity = solidity;
        }

    }

    public class PlayerBattleStats:BattleStats
    {
        private BoundedInt _energy;

        public WeaponInfo[] Weapons { get;}
        public uint CurrentWeaponId { get; set; }
        public WeaponInfo CurrentWeapon => Weapons[CurrentWeaponId];
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


        public PlayerBattleStats(Solidity solidity, int maxEnergy, int energyRegen, WeaponInfo[] weapons): base(solidity)
        {
            _energy = new BoundedInt(maxEnergy, 0, maxEnergy);
            EnergyRegen = energyRegen;
            Weapons = weapons;
            CurrentWeaponId = 0;
        }

    }

    public class EnemyBattleStats:BattleStats
    {
        public double Velocity { get; set; }
        public WeaponInfo Weapon { get; }

        public EnemyBattleStats(Solidity solidity, double velocity, WeaponInfo weapon) : base(solidity)
        {
            Velocity = velocity;
            Weapon = weapon;
        }
    }
}