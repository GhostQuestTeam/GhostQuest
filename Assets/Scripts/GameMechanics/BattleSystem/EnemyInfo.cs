using System;
using UnityEngine;

namespace HauntedCity.GameMechanics.BattleSystem
{
    public enum AttackType
    {
        Melee,
        Range,
        Kamikaze
    }
    
    [CreateAssetMenu(fileName = "New enemy", menuName = "Battle System/EnemyInfo")]
    public class EnemyInfo:ScriptableObject
    {
        public String Id;
        public String Name;
        public int Price;
        public AttackType attackType = AttackType.Kamikaze;
        public float DeathDelay = 0f;
        public float AttackRange = 1f;
        public float AttackCooldown = 1f;
        public int Score = 100;
        public double BonusDropProb = 0.5;
        public int MaxHealth;
        public int Defence;
        public float Velocity;
        public int Damage;
    }
}