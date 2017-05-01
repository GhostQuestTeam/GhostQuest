using System.Collections;
using UnityEngine;

namespace BattleSystem.ConcreteEnemyBehaviors
{
    [RequireComponent(typeof(EnemyBehavior))]
    public class SkullGhostBehavior : MonoBehaviour
    {
        public string ExplosionPrefabPath = "FX/SkullGhostExplosion";

        private BattleController _battleController;
        private BattleStats _battleStats;
        private Transform _jaw;
        private Transform _skull;

        private GameObject _explosionPrefab;
        private float _jawVelocity;
        private float _skullVelocity;


        void Start()
        {
            _explosionPrefab = Resources.Load(ExplosionPrefabPath) as GameObject;

            _battleController = GetComponent<EnemyBehavior>().BattleController;
            _battleStats = GetComponent<EnemyBehavior>().BattleStats;

            _battleController.OnDamage += TakeDamageAnimate;
            _battleController.OnDeath += DeathAnimate;


            _jaw = transform.Find("model/jaw");
            _skull = transform.Find("model/skull");
        }

        private void Update()
        {
            _jaw.localPosition += new Vector3(0, _jawVelocity, 0);
            _skull.localPosition += new Vector3(0, _skullVelocity, 0);
        }

        public void TakeDamageAnimate(int hpDelta)
        {
            if (_battleStats.Solidity.IsAlive())
            {
                StartCoroutine(TakeDamageAnimateCorutine());
            }
        }

        public void DeathAnimate()
        {
            _jawVelocity = -2f;
            _skullVelocity = 0.75f;
            var explosion = Instantiate(_explosionPrefab);
            explosion.transform.SetParent(transform);
        }

        public IEnumerator TakeDamageAnimateCorutine()
        {
            _jawVelocity = -0.15f;
            _skullVelocity = 0.2f;
            yield return new WaitForSeconds(0.07f);
            _jawVelocity = 0.15f;
            _skullVelocity = -0.2f;
            yield return new WaitForSeconds(0.07f);
            _jaw.localPosition = Vector3.zero;
            _skull.localPosition = Vector3.zero;
            _jawVelocity = 0;
            _skullVelocity = 0;
        }
    }
}