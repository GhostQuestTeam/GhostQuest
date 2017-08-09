using System.Security.Principal;
using HauntedCity.Utils;
using UnityEditor;

namespace HauntedCity.GameMechanics.SkillSystem
{
    public class PlayerCharacteristic
    {
        public const int MAX_VALUE = 100;
        public const int MIN_VALUE = 5;

        private BoundedInt _value;
        public int Delta { get; set; }

        public int DisplayValue
        {
            get { return _value.Val + Delta; }
        }

        public bool CanInc()
        {
            return DisplayValue < MAX_VALUE;
        }

        public bool CanDec()
        {
            return Delta > 0;
        }

        public void Confirm()
        {
            _value.Val += Delta;
        }

        public PlayerCharacteristic(int value = MIN_VALUE)
        {
            _value = new BoundedInt(MAX_VALUE, MIN_VALUE, value);
        }
        
        
    }
}