using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class EnemyBehavior : MonoBehaviour
    {
        private GameObject _followee;

        public EnemyBattleController BattleController { get; private set; }
        public EnemyBattleStats BattleStats;
        public float DeathDelay = 0f;

        // Use this for initialization
        void Awake()
        {
            _followee = GameObject.FindWithTag("Player");
            BattleStats.ResetHealth();
            BattleController = new EnemyBattleController(BattleStats);
            BattleController.OnDeath += () => StartCoroutine(Kill(DeathDelay));
        }

        // Update is called once per frame
        void Update()
        {
            if (_followee != null)
            {
                transform.LookAt(_followee.transform);
                transform.position = Vector3.MoveTowards(transform.position, _followee.transform.position,
                    BattleStats.Velocity);
            }
            else
            {
                Debug.logger.Log(gameObject.name + ": my followee is null!!!");
            }
        }


        //TODO Заменить на триггер
        public void OnCollisionEnter(Collision collision)
        {
            var shell = collision.gameObject.GetComponent<ShellBehavior>();
            if (shell != null)
            {
                BattleController.TakeDamage(shell.ShellInfo);
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                var player =collision. gameObject.GetComponent<PlayerBattleBehavior>();
                player.BattleController.TakeDamage(BattleStats.Shell);
                Destroy(gameObject);
            }
        }

        public IEnumerator Kill(float delay)
        {
            BattleStats.Velocity = 0f;
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }

    }
}