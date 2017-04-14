using Utils;

namespace BattleSystem
{
    //Уровень просчности объекта
    public class Solidity
    {
        private BoundedInt _health = new BoundedInt();

        public int RegenPoints { get; set; }

        public uint MaxHealth
        {
            get { return (uint) _health.Max; }
            set { _health.Max = (int) value; }
        }

        public int CurrentHealth {
            get { return _health.Val; }
            private set { _health.Val = value; }
        }
        public int Defence { get; set; }

        private void _changeHealth(int healthDelta)
        {
            CurrentHealth += healthDelta;
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = (int) MaxHealth;
            }
        }

        public void Attack(int damage)
        {
            if (damage <= Defence)
            {
                damage = 0;
            }
            else
            {
                damage -= Defence;
            }
            CurrentHealth -= damage;
        }

        public void Heal(int healPoints)
        {
            if (IsAlive())
            {
                CurrentHealth += healPoints;
            }
        }

        public void Regenerate()
        {
            Heal(RegenPoints);
        }

        public bool IsAlive()
        {
            return CurrentHealth != 0;
        }

        public Solidity(uint maxHealth, int defence, int regenPoints = 0)
        {
            MaxHealth = maxHealth;
            Defence = defence;
            RegenPoints = regenPoints;
            CurrentHealth = (int) MaxHealth;
        }

        public Solidity(uint maxHealth, int defence, int regenPoints, int currentHealth)
        {
            MaxHealth = maxHealth;
            Defence = defence;
            RegenPoints = regenPoints;
            CurrentHealth = currentHealth;
        }
    }
}