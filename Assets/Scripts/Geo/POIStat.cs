using HauntedCity.GameMechanics.Main;

namespace HauntedCity.Geo
{
    public abstract class POIStat
    {
        
        public abstract int MaxValue { get; }
        public abstract int Price { get; }
        public abstract int MaxLevel { get; }
        
        public int Level { get; protected set;}
        public int Value { get; protected set;}

        public bool CanUpgrade()
        {
            return (Level < MaxLevel) && Price <= GameController.GameStats.Money;
        }

        public bool TryUpgrade()
        {
            if (!CanUpgrade()) return false;
            Level++;
            GameController.GameStats.TryGetMoney(Price);
            return true;
        }

        protected POIStat(int level =1, int value =0)
        {
            Level = level;
            Value = value;
            
            if (Level > MaxLevel) Level = MaxLevel;
            if (Value > MaxValue) Value = MaxValue;
            if (Level < 1) Level = 1;
            if (Value < 0) Value = 0;
        }
        
        
    }
}