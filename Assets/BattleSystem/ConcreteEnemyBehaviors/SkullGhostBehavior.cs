using UnityEngine;

namespace BattleSystem.ConcreteEnemyBehaviors
{

    [RequireComponent(typeof(EnemyBehavior))]
    public class SkullGhostBehavior:MonoBehaviour
    {
        public int Damage = 30;

        private ShellInfo _shellStub;

        void Start()
        {
            _shellStub = new ShellInfo(Damage, 0);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            collision.gameObject.GetComponent<PlayerBattleBehavior>().BattleController.TakeDamage(_shellStub);
            Destroy(gameObject);
        }
    }
}