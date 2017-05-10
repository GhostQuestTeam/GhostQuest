using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(fileName = "New weapon", menuName = "Battle System/Weapon")]
    public class Weapon:ScriptableObject
    {
        public GameObject Prefab;
        public Sprite Sprite;
        public int Damage;
        public float Velocity;
        public string Id;
        public int ShootCost;
        public float Cooldown;
        public float Ttl = 1f;
    }
}