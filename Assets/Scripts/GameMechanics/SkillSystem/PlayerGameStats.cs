using System;
using System.Collections.Generic;
using System.Linq;
using GameSparks.Core;
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

        public event Action OnAttributeChange;

        private BoundedInt _baseSurviability;
        private BoundedInt _baseEndurance;
        private BoundedInt _basePower;

        private int _surviabilityDelta;
        private int _enduranceDelta;
        private int _powerDelta;

        public int UpgradePoints;
        private const int UPGRADE_POINTS_PER_LEVEL = 5;


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
                    .AddNumber("exp", CurrentExp);
                
                return result;
            }
            set
            {
                UpgradePoints = (int) value.GetInt("upgradePoints");

                _baseSurviability.Val = (int) value.GetInt("survivability");
                _baseEndurance.Val = (int) value.GetInt("ebdurance");
                _basePower.Val = (int) value.GetInt("power");

                _surviabilityDelta = 0;
                _enduranceDelta = 0;
                _powerDelta = 0;

                Level = (int) value.GetInt("level");
                CurrentExp = (int) value.GetInt("exp");
                
                _UpdateExpToNextLevel();


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
            ExpToLevel += 100 * Level * Level + 50 * Level + 1000;
        }

        private void _NextLevel()
        {
            Level++;
            UpgradePoints += UPGRADE_POINTS_PER_LEVEL;
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

        public PlayerGameStats()
        {
            Level = 1;
            CurrentExp = 0;
            _UpdateExpToNextLevel();

            _baseSurviability = new BoundedInt(100, 5, 5);
            _baseEndurance = new BoundedInt(100, 5, 5);
            _basePower = new BoundedInt(100, 5, 5);

            SkillPoints = SKILL_POINTS_PER_LEVEL;

            UpgradePoints = UPGRADE_POINTS_PER_LEVEL;
        }
    }
}