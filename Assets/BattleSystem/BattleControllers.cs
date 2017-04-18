using System;

namespace BattleSystem
{
    public interface IShooter
    {
        void Shoot(WeaponInfo weapon);
    }

    public interface IWeaponSpeedController
    {
        bool CanShoot(WeaponInfo weapon);
        void BlockWeapon(WeaponInfo weapon, float duration);
    }


    public abstract class BattleController
    {
        public event Action OnDeath;
        public event Action<int> OnDamage;

        protected BattleStats battleStats;

        public void TakeDamage(ShellInfo shell)
        {
            int damage = shell.Damage;
            if (shell.Effect != null)
            {
                shell.Effect(battleStats);
            }
            int startedHealth = battleStats.CurrentHealth;
            battleStats.Solidity.Attack(damage);
            int deltaHealth = startedHealth - battleStats.CurrentHealth;
            if (deltaHealth != 0)
            {
                if (OnDamage != null)
                {
                    OnDamage(deltaHealth);
                }
            }
            if (!battleStats.Solidity.IsAlive())
            {
                if (OnDeath != null)
                {
                    OnDeath();
                }
            }
        }

        public virtual void Regenerate()
        {
            battleStats.Solidity.Regenerate();
        }
    }

    public class PlayerBattleController : BattleController
    {
        private IShooter _shooter;
        private IWeaponSpeedController _weaponSpeedController;
        public event Action<int> OnEnergyChanged;

        private const float _TOLERANCE = 0.00000001f;

        public PlayerBattleStats BattleStats
        {
            get { return battleStats as PlayerBattleStats; }
        }

        public void TryShoot()
        {
            if (BattleStats.CurrentEnergy < BattleStats.CurrentWeapon.ShootCost) return;
            if (!_weaponSpeedController.CanShoot(BattleStats.CurrentWeapon)) return;
            _shooter.Shoot(BattleStats.CurrentWeapon);
            BattleStats.CurrentEnergy -= BattleStats.CurrentWeapon.ShootCost;
            if (OnEnergyChanged != null)
            {
                OnEnergyChanged(BattleStats.CurrentWeapon.ShootCost);
            }
            if (Math.Abs(BattleStats.CurrentWeapon.Cooldown) > _TOLERANCE)
            {
                _weaponSpeedController.BlockWeapon(BattleStats.CurrentWeapon, BattleStats.CurrentWeapon.Cooldown);
            }
        }

        public override void Regenerate()
        {
            base.Regenerate();
            int oldEnergy = BattleStats.CurrentEnergy;
            BattleStats.CurrentEnergy += BattleStats.EnergyRegen;
            int energyDelta = BattleStats.CurrentEnergy - oldEnergy;
            if (energyDelta != 0)
            {
                if (OnEnergyChanged != null)
                {
                    OnEnergyChanged(energyDelta);
                }
            }
        }

        public PlayerBattleController(PlayerBattleStats stats, IShooter shooter, IWeaponSpeedController speedController)
        {
            battleStats = stats;
            _shooter = shooter;
            _weaponSpeedController = speedController;
        }

    }

    public class EnemyBattleController : BattleController
    {
        public EnemyBattleStats BattleStats
        {
            get { return battleStats as EnemyBattleStats; }
        }

        public EnemyBattleController(EnemyBattleStats stats)
        {
            battleStats = stats;
        }
    }
}