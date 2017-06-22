using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HauntedCity.GameMechanics.BattleSystem
{
    public class ShellBehavior : MonoBehaviour
    {
        [SerializeField] public Weapon Weapon;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(DestroyAfterTimeout());
        }

        // Update is called once per frame
        void Update()
        {
        }

        public IEnumerator DestroyAfterTimeout()
        {
            yield return new WaitForSeconds(Weapon.Ttl);
            Destroy(gameObject);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) return;
            Destroy(gameObject);
        }


        public void Launch(Vector3 startPosition, Vector3 velocityVector)
        {
            gameObject.transform.position = startPosition;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = velocityVector * Weapon.Velocity;
        }
    }
}