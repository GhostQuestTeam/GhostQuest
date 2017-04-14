using System.Security.Policy;

namespace BattleSystem
{
    //Уровень просчности объекта
    public class Solidity
    {
        public int RegenPoints { get; set; }
        public uint MaxHealth { get; set; }
        public int CurrentHealth { get; private set; }
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
                CurrentHealth = (int)MaxHealth;
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
            _changeHealth(-damage);
        }

        public void Heal(int healPoints)
        {
            if (IsAlive())
            {
                _changeHealth(healPoints);
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
            CurrentHealth = (int)MaxHealth;
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