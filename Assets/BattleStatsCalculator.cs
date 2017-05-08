using SkillSystem;
using BattleSystem;

//Вычисление боевых характеристик, на основе прокаченных скиллов
public static class BattleStatsCalculator
{
    public const float HEALTH_PER_SURVIABIBILITY = 5f;
    public const float HEALTH_REGEN_PER_SURVIABIBILITY = 0.025f;

    public const float ENERGY_PER_ENDURANCE = 5f;
    public const float ENERGY_REGEN_PER_ENDURANCE = 0.04f;

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
        //TODO
    }

    private static void _ApplySkillModifiers(PlayerBattleStats battleStats, PlayerGameStats gameStats)
    {
        foreach (var skill in gameStats.Skills.Values)
        {
            if (skill is StatsModifier)
            {
                (skill as StatsModifier).ApplyModifier(battleStats);
            }
        }
    }

    public static PlayerBattleStats CalculateBattleStats(PlayerGameStats gameStats)
    {
        var solidity = new Solidity(50, 0);
        var weapons = new WeaponInfo[2];

        weapons[0] = WeaponLoader.LoadWeapon("sphere");
        weapons[1] = WeaponLoader.LoadWeapon("laser");

        var battleStats = new PlayerBattleStats(solidity, 50, 1, weapons);
        _ApplySurvivabilityModifiers(battleStats, gameStats);
        _ApplyEnduranceModifiers(battleStats, gameStats);
        _ApplyPowerModifiers(battleStats, gameStats);
        _ApplySkillModifiers(battleStats, gameStats);


        battleStats.ResetHealth();
        battleStats.CurrentEnergy = battleStats.MaxEnergy;
        return battleStats;
    }
}