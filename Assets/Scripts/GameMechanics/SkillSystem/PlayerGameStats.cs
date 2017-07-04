using System;
using System.Collections.Generic;
using System.Linq;
using GameSparks.Core;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.Utils;

namespace HauntedCity.GameMechanics.SkillSystem
{
    public class PlayerGameStats
    {
        #region PlayerAttributes

        public enum PlayerAttributes
        {
            Survivability,
            Endurance,
            Power
        }

        public readonly List<string> DEFAULT_WEAPONS = new List<string>() {"sphere", "air_bolt"}; 
        
        public event Action OnAttributeChange;
        public event Action OnAttributesUpgrade;
        public event Action OnCoinChange;
        public event Action<Weapon> OnWeaponBuy;


        private BoundedInt _baseSurviability;
        private BoundedInt _baseEndurance;
        private BoundedInt _basePower;

        private int _surviabilityDelta;
        private int _enduranceDelta;
        private int _powerDelta;

        public int UpgradePoints;
        private const int UPGRADE_POINTS_PER_LEVEL = 5;

        public int POIs { get;  set; }

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

        public int Survivability
        {
            get { return _baseSurviability.Val + _surviabilityDelta; }
        }

        public int Endurance
        {
            get { return _baseEndurance.Val + _enduranceDelta; }
        }

        public int Power
        {
            get { return _basePower.Val + _powerDelta; }
        }

        public GSRequestData GSData
        {
            get
            {
                var result = new GSRequestData();

                int pointsToSave = UpgradePoints;
                int survivabilityToSave = Survivability;
                int enduranceToSave = Endurance;
                int powerToSave = Power;

                pointsToSave += _enduranceDelta + _powerDelta + _surviabilityDelta;
                survivabilityToSave -= _surviabilityDelta;
                enduranceToSave -= _enduranceDelta;
                powerToSave -= _powerDelta;

                result.AddNumber("upgradePoints", pointsToSave)
                    .AddNumber("survivability", survivabilityToSave)
                    .AddNumber("endurance", enduranceToSave)
                    .AddNumber("power", powerToSave)
                    .AddNumber("level", Level)
                    .AddNumber("exp", CurrentExp)
                    .AddNumber("money", Money)
                    .AddStringList("currentWeapons", CurrentWeapons)
                    .AddStringList("allowableWeapons", AllowableWeapons);

                return result;
            }
            set
            {
                UpgradePoints = value.GetInt("upgradePoints") ?? 5;

                _baseSurviability.Val = value.GetInt("survivability") ?? 5;
                _baseEndurance.Val = value.GetInt("endurance") ?? 5;
                _basePower.Val = value.GetInt("power") ?? 5;

                _surviabilityDelta = 0;
                _enduranceDelta = 0;
                _powerDelta = 0;

                Level = value.GetInt("level") ?? 1;
                CurrentExp = value.GetInt("exp") ?? 0;
                Money = value.GetInt("money") ?? 10000;

                AllowableWeapons = value.GetStringList("allowableWeapons") ?? new List<string>(DEFAULT_WEAPONS);
                CurrentWeapons = value.GetStringList("currentWeapons") ?? new List<string>(DEFAULT_WEAPONS);
                POIs = value.GetInt("numOfPOIs") ?? 0;

                _UpdateExpToNextLevel();
            }
        }


        public int GetAttribute(PlayerAttributes attribute)
        {
            switch (attribute)
            {
                case PlayerAttributes.Survivability:
                    return Survivability;
                case PlayerAttributes.Endurance:
                    return Endurance;
                case PlayerAttributes.Power:
                    return Power;
                default:
                    throw new ArgumentOutOfRangeException("attribute", attribute, null);
            }
        }

        public void IncAttribute(PlayerAttributes attribute)
        {
            if (UpgradePoints == 0) return;
            UpgradePoints--;
            switch (attribute)
            {
                case PlayerAttributes.Survivability:
                    _surviabilityDelta++;
                    break;
                case PlayerAttributes.Endurance:
                    _enduranceDelta++;
                    break;
                case PlayerAttributes.Power:
                    _powerDelta++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("attribute", attribute, null);
            }
            if (OnAttributeChange != null)
            {
                OnAttributeChange();
            }
        }

        public void DecAttribute(PlayerAttributes attribute)
        {
            switch (attribute)
            {
                case PlayerAttributes.Survivability:
                    _DecAttributeDelta(ref _surviabilityDelta);
                    break;
                case PlayerAttributes.Endurance:
                    _DecAttributeDelta(ref _enduranceDelta);
                    break;
                case PlayerAttributes.Power:
                    _DecAttributeDelta(ref _powerDelta);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("attribute", attribute, null);
            }
            if (OnAttributeChange != null)
            {
                OnAttributeChange();
            }
        }

        private void _DecAttributeDelta(ref int attributeDelta)
        {
            if (attributeDelta == 0) return;
            attributeDelta--;
            UpgradePoints++;
        }

        public void ConfirmUpgrades()
        {
            _baseSurviability.Val += _surviabilityDelta;
            _baseEndurance.Val += _enduranceDelta;
            _basePower.Val += _powerDelta;

            _surviabilityDelta = 0;
            _enduranceDelta = 0;
            _powerDelta = 0;

            if (OnAttributesUpgrade != null)
            {
                OnAttributesUpgrade();
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

        #region LevelsAndExp

        public int Level { get; private set; }
        public int CurrentExp { get; private set; }
        public int ExpToLevel { get; private set; }

        private void _UpdateExpToNextLevel()
        {
            ExpToLevel = 80 * Level * Level * Level + 50 * Level + 1000;
        }

        private void _NextLevel()
        {
            Level++;
            UpgradePoints += UPGRADE_POINTS_PER_LEVEL;
            CurrentExp = 0;
            _UpdateExpToNextLevel();
        }

        public void AddExp(int exp)
        {
            CurrentExp += exp;
            while (CurrentExp > ExpToLevel)
            {
                _NextLevel();
            }
        }

        #endregion

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
            Level = 1;
            Money = 10000;
            CurrentExp = 0;
            _UpdateExpToNextLevel();
            CurrentWeapons = new List<string>(DEFAULT_WEAPONS);
            AllowableWeapons = new List<string>(DEFAULT_WEAPONS);

            _baseSurviability = new BoundedInt(100, 5, 5);
            _baseEndurance = new BoundedInt(100, 5, 5);
            _basePower = new BoundedInt(100, 5, 5);

            SkillPoints = SKILL_POINTS_PER_LEVEL;

            UpgradePoints = UPGRADE_POINTS_PER_LEVEL;
        }
    }
}