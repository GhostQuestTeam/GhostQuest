using HauntedCity.GameMechanics.Main;
using UnityEditor;

namespace HauntedCity.Geo
{
    public class POIMoney:POIStat
    {
        public override int MaxValue
        {
            get { return Level * 90; }
        }

        public override int Price
        {
            get { return Level * 500; }
        }

        public override int MaxLevel
        {
            get { return 5; }
        }

        public bool CanTakeMoney()
        {
            return Value != 0;
        }
        
        public void TakeMoney() 
        {
            GameController.GameStats.Money += Value;
            Value = 0;
        }
        
        public POIMoney(int level =1, int value =0):base(level,value){}

    }
}