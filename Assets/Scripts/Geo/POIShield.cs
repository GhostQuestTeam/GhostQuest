using System.Collections.Generic;
using HauntedCity.GameMechanics.Main;

namespace HauntedCity.Geo
{
    public class POIShield:POIStat
    {
        public const int DAMAGE = 300;
        public const int ATTACK_PRICE = 150;
        public const int RESTORE_PRICE = 300;
        
       
        public override int MaxValue
        {
            get { return Level * 300; }
        }

        public override int Price
        {
            get { return Level * 500; }
        }

        public override int MaxLevel
        {
            get { return  5; }
        }

        public bool CanAttack()
        {
            return (Value > 0) && GameController.GameStats.Money >= ATTACK_PRICE;
        }
        
        public bool TryAttack()
        {
            if (!CanAttack()) return false;
            GameController.GameStats.TryGetMoney(ATTACK_PRICE);
            Value -= DAMAGE;
            if (Value < 0) Value = 0;
            return true;
        }
        
        public bool CanRestore()
        {
            return (Value < MaxValue) && GameController.GameStats.Money >= RESTORE_PRICE;
        }
        
        public bool TryRestore()
        {
            if (!CanRestore()) return false;
            GameController.GameStats.TryGetMoney(RESTORE_PRICE);
            Value += DAMAGE;
            if (Value > MaxValue) Value = MaxValue;
            return true;
        }
        
        public POIShield(int level =1, int value =0):base(level,value){}

    }
}