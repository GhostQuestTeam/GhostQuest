using System;
using System.Collections.Generic;
using Utils;
using SkillSystem;

public class PlayerGameStats
{
    #region PlayerAttributes

    public enum PlayerAttributes
    {
        Survivability,
        Endurance,
        Power
    }

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
    public Dictionary<string, AbstractSkill> Skills = new Dictionary<string, AbstractSkill>();
    public int SkillPoints { get; private set; }
    public const int SKILL_POINTS_PER_LEVEL = 1;

    public void AddSkill(AbstractSkill skill)
    {
        if(Skills.ContainsKey(skill.name) ) return;
        if (SkillPoints == 0 ) return;
        Skills.Add(skill.name, skill);
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