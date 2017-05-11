using System;
using Utils;

namespace BattleSystem
{
    //Уровень просчности объекта
    [Serializable]
    public class Solidity
    {

        public int RegenPoints;
        public int Defence;
        public uint MaxHealth;
        public int CurrentHealth { get; private set; }


        private void _changeHealth(int healthDelta)
        {
            CurrentHealth += healthDelta;
            CurrentHealth = BoundedInt.Clamp(CurrentHealth, (int) MaxHealth);
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
            _changeHealth( -damage);
        }

        public void Kill()
        {
            CurrentHealth = 0;
        }

        public void Heal(int healPoints)
        {
            if (IsAlive())
            {
                _changeHealth(healPoints);
            }
        }

        public void ResetHealth()
        {
            CurrentHealth = (int)MaxHealth;
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