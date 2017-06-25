using System;
using UnityEngine;
using Zenject;

namespace HauntedCity.GameMechanics.BattleSystem
{
    public class Bonus:MonoBehaviour
    {
        private PlayerBattleController _player;

        public enum BonusType
        {
            Health,
            Energy
        }

        public BonusType bonusType;
        public int Value;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>().BattleController;
        }

        public void OnRay()
        {
            Debug.Log("Zis iz worgs!!");
            switch (bonusType)
            {
                case BonusType.Health:
                    _player.Heal(Value);
                    break;
                case BonusType.Energy:
                    _player.RestoreEnergy(Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }            
            Destroy(gameObject);

        }
    }
}