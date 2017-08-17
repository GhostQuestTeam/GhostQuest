using System;
using System.Collections.Generic;
using System.Linq;
using GameSparks.Core;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.Geo;
using HauntedCity.Networking;
using HauntedCity.Utils;
using UnityEngine;

namespace HauntedCity.GameMechanics.SkillSystem
{
    public class PlayerGameStats
    {
        #region PlayerAttributes

        public StorageService StorageService;

        public enum PlayerAttributes
        {
            Survivability,
            Endurance,
            Power
        }

        public readonly List<string> DEFAULT_WEAPONS = new List<string>() {"sphere", "air_bolt"};

        public event Action OnCoinChange;
        public event Action<Weapon> OnWeaponBuy;


        private const int UPGRADE_POINTS_PER_LEVEL = 5;

        public Dictionary<string, PointOfInterestData> POIs { get; set; }

        public List<string> AllowableWeapons;
        public List<string> CurrentWeapons;

        private int _money;


        public int Money
        {
            get { return _money; }
            set
            {
                _money = value;
                if (OnCoinChange != null)
                {
                    OnCoinChange();
                }
            }
        }

        public PlayerCharacteristicManager CharacteristicManager { get; private set; }

        public int Survivability
        {
            get { return CharacteristicManager.GetCharacteristic(PlayerCharacteristics.Survivability); }
        }

        public int Endurance
        {
            get { return CharacteristicManager.GetCharacteristic(PlayerCharacteristics.Endurance); }
        }

        public int Power
        {
            get { return CharacteristicManager.GetCharacteristic(PlayerCharacteristics.Power); }
        }

        public int SurvivabilityDelta
        {
            get { return CharacteristicManager.GetDelta(PlayerCharacteristics.Survivability); }
        }

        public int EnduranceDelta
        {
            get { return CharacteristicManager.GetDelta(PlayerCharacteristics.Endurance); }
        }

        public int PowerDelta
        {
            get { return CharacteristicManager.GetDelta(PlayerCharacteristics.Power); }
        }

        public GSRequestData GSData
        {
            set
            {
                CharacteristicManager.SetGSData(value);

                PlayerExperience = new Experience(
                    value.GetInt("level") ?? 1,
                    value.GetInt("exp") ?? 0
                );
                Money = value.GetInt("money") ?? 10000;

                AllowableWeapons = value.GetStringList("allowableWeapons") ?? new List<string>(DEFAULT_WEAPONS);
                CurrentWeapons = value.GetStringList("currentWeapons") ?? new List<string>(DEFAULT_WEAPONS);
                var GS_POIs = value.GetGSDataList("POIs");
                if (GS_POIs != null)
                {
                    foreach (var gsPoi in GS_POIs)
                    {
                        Debug.Log("\n" + gsPoi.JSON + "\n");
                        var poi = new PointOfInterestData(gsPoi);
                        POIs[poi.Poid] = poi;
                    }
                }
            }
        }

        #endregion

        #region Skills

        public List<AbstractSkill> Skills = new List<AbstractSkill>();
        private HashSet<string> _learnedSkills;
        public int SkillPoints { get; private set; }
        public const int SKILL_POINTS_PER_LEVEL = 1;

        public void AddSkill(AbstractSkill skill)
        {
            if (_learnedSkills.Contains(skill.name)) return;
            if (SkillPoints == 0) return;
            if (skill.Dependencies.Any(dependency => !_learnedSkills.Contains(dependency))) return;

            _learnedSkills.Add(skill.name);
            Skills.Add(skill);
        }

        #endregion

        public Experience PlayerExperience;

        public void AddExp(int exp)
        {
            var earnedLevels = PlayerExperience.AddExp(exp);
            CharacteristicManager.AccruePoints(earnedLevels);
        }

        public bool TryGetMoney(int amount)
        {
            if (amount > Money) return false;
            Money -= amount;
            return true;
        }

        public bool TryBuyWeapon(Weapon weapon)
        {
            if (AllowableWeapons.Contains(weapon.Id)) return false;
            if (weapon.Cost > Money) return false;

            Money -= weapon.Cost;
            AllowableWeapons.Add(weapon.Id);
            if (OnWeaponBuy != null)
            {
                OnWeaponBuy(weapon);
            }
            return true;
        }

        public PlayerGameStats()
        {
            
            Money = 10000;
            PlayerExperience = new Experience();
            CurrentWeapons = new List<string>(DEFAULT_WEAPONS);
            AllowableWeapons = new List<string>(DEFAULT_WEAPONS);

            CharacteristicManager = new PlayerCharacteristicManager();

            SkillPoints = SKILL_POINTS_PER_LEVEL;
            POIs = new Dictionary<string, PointOfInterestData>();

        }
    }
}