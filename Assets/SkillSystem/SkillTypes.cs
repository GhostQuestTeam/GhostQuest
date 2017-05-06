using System;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

namespace SkillSystem
{
    public class AbstractSkill : ScriptableObject
    {
        public Sprite SkillSprite;
        public string Description = "";
        public string[] Dependencies;
    }

    public enum StatModifierType
    {
        ADDITIVE,
        MULTIPLICATIVE
    }

    public enum PlayerStatType
    {
        HEALTH,
        HEALTH_REGEN,
        ENERGY,
        ENERGY_REGEN,
        DEFENCE
    }

    [Serializable]
    public class SingleStatModifier
    {
        [SerializeField] public StatModifierType statModifierType;
        public float modifierValue;
        public PlayerStatType StatType;

        #region ApplyModifierInternal
        private void _applyAdditiveModifier(PlayerBattleStats playerBattleStats)
        {
            switch (StatType)
            {
                case PlayerStatType.HEALTH:
                    playerBattleStats.Solidity.MaxHealth += (uint) modifierValue;
                    break;
                case PlayerStatType.HEALTH_REGEN:
                    playerBattleStats.Solidity.RegenPoints += (int) modifierValue;
                    break;
                case PlayerStatType.ENERGY:
                    playerBattleStats.MaxEnergy += (int) modifierValue;
                    break;
                case PlayerStatType.ENERGY_REGEN:
                    playerBattleStats.EnergyRegen += (int) modifierValue;
                    break;
                case PlayerStatType.DEFENCE:
                    playerBattleStats.Solidity.Defence += (int) modifierValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void _applyMultiplicativeModifier(PlayerBattleStats battleStats)
        {
            switch (StatType)
            {
                case PlayerStatType.HEALTH:
                    battleStats.Solidity.MaxHealth =
                        (uint) (battleStats.Solidity.MaxHealth * modifierValue);
                    break;
                case PlayerStatType.HEALTH_REGEN:
                    battleStats.Solidity.RegenPoints = (int) (battleStats.Solidity.RegenPoints * modifierValue);
                    break;
                case PlayerStatType.ENERGY:
                    battleStats.MaxEnergy = (int)( modifierValue * battleStats.MaxEnergy );
                    break;
                case PlayerStatType.ENERGY_REGEN:
                    battleStats.EnergyRegen += (int)( modifierValue * battleStats.EnergyRegen );
                    break;
                case PlayerStatType.DEFENCE:
                    battleStats.Solidity.Defence += (int) (modifierValue * battleStats.Solidity.Defence);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        public PlayerBattleStats ApplyModifier(PlayerBattleStats playerBattleStats)
        {
            switch (statModifierType)
            {
                case StatModifierType.ADDITIVE:
                    _applyAdditiveModifier(playerBattleStats);
                    break;
                case StatModifierType.MULTIPLICATIVE:
                    _applyMultiplicativeModifier(playerBattleStats);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return playerBattleStats;
        }
    }

    [CreateAssetMenu(fileName = "StatsModifier", menuName = "Player Skills/StatsModifier")]
    public class StatsModifier : AbstractSkill
    {
        [SerializeField] public SingleStatModifier[] StatModifiers;
    }

    [CreateAssetMenu(fileName = "CastUnlocker", menuName = "Player Skills/CastUnlocker")]
    public class CastUnlocker : AbstractSkill
    {
        public string UnlockedCastId;
    }
}