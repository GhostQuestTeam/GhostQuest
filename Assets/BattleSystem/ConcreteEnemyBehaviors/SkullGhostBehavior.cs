using System.Collections;
using UnityEngine;

namespace BattleSystem.ConcreteEnemyBehaviors
{

    [RequireComponent(typeof(EnemyBehavior))]
    public class SkullGhostBehavior:MonoBehaviour
    {
        private BattleController _battleController;
        private Transform _jaw;
        private Transform _skull;

        private float _jawVelocity;
        private float _skullVelocity;


        void Start()
        {
            _battleController = GetComponent<EnemyBehavior>().BattleController;
            _battleController.OnDamage += TakeDamageAnimate;
            _jaw = transform.Find("model/jaw");
            _skull = transform.Find("model/skull");

        }

        private void Update()
        {
            _jaw.localPosition += new Vector3(0,_jawVelocity, 0);
            _skull.localPosition += new Vector3(0,_skullVelocity, 0);
        }

        public void TakeDamageAnimate(int hpDelta)
        {
            StartCoroutine(TakeDamageAnimateCorutine());
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