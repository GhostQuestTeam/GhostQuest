using SkillSystem;
using BattleSystem;

//Вычисление боевых характеристик, на основе прокаченных скиллов
public static class BattleStatsCalculator
{
    #region Constants
    public const float HEALTH_PER_SURVIABIBILITY = 5f;
    public const float HEALTH_REGEN_PER_SURVIABIBILITY = 0.025f;

    public const float ENERGY_PER_ENDURANCE = 5f;
    public const float ENERGY_REGEN_PER_ENDURANCE = 0.04f;

    public const float DAMAGE_MODIFIER_PER_POWER = 0.04f;
    #endregion

    #region Apply modifiers

    private static void _ApplySurvivabilityModifiers(PlayerBattleStats battleStats, PlayerGameStats gameStats)
    {
        battleStats.Solidity.MaxHealth += (uint) (gameStats.Survivability* HEALTH_PER_SURVIABIBILITY);
        battleStats.Solidity.RegenPoints += (int) (gameStats.Survivability * HEALTH_REGEN_PER_SURVIABIBILITY);
    }

    private static void _ApplyEnduranceModifiers(PlayerBattleStats battleStats, PlayerGameStats gameStats)
    {
        battleStats.MaxEnergy += (int) (gameStats.Endurance* ENERGY_PER_ENDURANCE);
        battleStats.EnergyRegen += (int) (gameStats.Endurance * ENERGY_REGEN_PER_ENDURANCE);
    }

    private static void _ApplyPowerModifiers(PlayerBattleStats battleStats, PlayerGameStats gameStats)
    {
        battleStats.DamageModifier += DAMAGE_MODIFIER_PER_POWER * gameStats.Power;
    }

    private static void _ApplySkillModifiers(PlayerBattleStats battleStats, PlayerGameStats gameStats)
    {
        foreach (var skill in gameStats.Skills)
        {
            if (skill is StatsModifier)
            {
                (skill as StatsModifier).ApplyModifier(battleStats);
            }
        }
    }
    #endregion

    public static PlayerBattleStats CalculateBattleStats(PlayerGameStats gameStats)
    {
        var solidity = new Solidity(50, 0);
        var weapons = new Weapon[4];

        weapons[0] = WeaponLoader.LoadWeapon("sphere");
        weapons[1] = WeaponLoader.LoadWeapon("orb_1");
        weapons[2] = WeaponLoader.LoadWeapon("aura_1");
        weapons[3] = WeaponLoader.LoadWeapon("fireball_1");

        var battleStats = new PlayerBattleStats(solidity, 50, 1, 1f ,weapons);
        _ApplySurvivabilityModifiers(battleStats, gameStats);
        _ApplyEnduranceModifiers(battleStats, gameStats);
        _ApplyPowerModifiers(battleStats, gameStats);
        _ApplySkillModifiers(battleStats, gameStats);


        battleStats.ResetHealth();
        battleStats.CurrentEnergy = battleStats.MaxEnergy;
        return battleStats;
    }
}