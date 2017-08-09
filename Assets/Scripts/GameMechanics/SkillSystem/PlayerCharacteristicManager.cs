using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using GameSparks.Core;
using HauntedCity.Utils;

namespace HauntedCity.GameMechanics.SkillSystem
{
    public class PlayerCharacteristicManager:Model
    {
       
        private readonly Dictionary<PlayerCharacteristics, PlayerCharacteristic> _characteristics =
            new Dictionary<PlayerCharacteristics, PlayerCharacteristic>();

        public int UpgradePoints { get; private set; }

        private string _GetGsCharacteristicName(PlayerCharacteristics characteristicType)
        {
            return characteristicType.ToString().ToLower();//TODO Cделать полное соответствие названий на сервере и клиенте
        }
        
        public void AccruePoints(int times)
        {
            UpgradePoints += 5 * times;
        }  
        
        public int GetCharacteristic(PlayerCharacteristics characteristic)
        {
            return _characteristics[characteristic].DisplayValue;
        }
        
        public int GetDelta(PlayerCharacteristics characteristic)
        {
            return _characteristics[characteristic].Delta;
        }

        public bool TryIncCharacteristic(PlayerCharacteristics characteristicType)
        {
            var characteristic = _characteristics[characteristicType];
            if (!CanIncCharacteristic(characteristicType)) return false;
            characteristic.Delta++;
            UpgradePoints--;
            _NotifyChanges();
            return true;
        }
        
        public bool TryDecCharacteristic(PlayerCharacteristics characteristicType)
        {
            var characteristic = _characteristics[characteristicType];
            if (!CanDecCharacteristic(characteristicType)) return false;
            characteristic.Delta--;
            UpgradePoints++;
            _NotifyChanges();
            return true;
        }

        public void ConfirmUpgrades()
        {
            _characteristics.Values.ToList().ForEach((p) => p.Confirm());
            _NotifyChanges();
        }

        public bool CanDecCharacteristic(PlayerCharacteristics characteristicType)
        {
            return _characteristics[characteristicType].CanDec();
        }

        public bool CanIncCharacteristic(PlayerCharacteristics characteristicType)
        {
            return _characteristics[characteristicType].CanInc() && (UpgradePoints > 0);
        }

        
        public void SetGSData(GSData gsData)
        {
            UpgradePoints = gsData.GetInt("upgradePoints") ?? 5;
            foreach (PlayerCharacteristics characteristic in Enum.GetValues(typeof(PlayerCharacteristics)))
            {
                var characteristicValue = gsData.GetInt(_GetGsCharacteristicName(characteristic)) ?? 0;
                _characteristics[characteristic] = new PlayerCharacteristic(characteristicValue);
            }
            _NotifyChanges();
        }
        
        public PlayerCharacteristicManager()
        {
            foreach (PlayerCharacteristics characteristic in Enum.GetValues(typeof(PlayerCharacteristics)))
            {
                _characteristics.Add(characteristic, new PlayerCharacteristic());
            }
        }

        
        
        public PlayerCharacteristicManager(GSData gsData):this()
        {
            SetGSData(gsData);
        }
    }
}