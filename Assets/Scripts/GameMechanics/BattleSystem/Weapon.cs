using UnityEngine;
using HauntedCity.GameMechanics.ItemSystem;

namespace HauntedCity.GameMechanics.BattleSystem
{
    [CreateAssetMenu(fileName = "New weapon", menuName = "Battle System/Weapon")]
    public class Weapon : UsableItem
    {
        public GameObject Prefab;
        public Sprite Sprite;
        public int BaseDamage;
        public float Velocity;
        public string Id;
        public int ShootCost{get { return UseCost; }}
        public float Ttl = 1f;

        [HideInInspector] public float DamageModifier = 1f;

        public int Damage
        {
            get { return (int) (BaseDamage * DamageModifier); }
        }
    }
}