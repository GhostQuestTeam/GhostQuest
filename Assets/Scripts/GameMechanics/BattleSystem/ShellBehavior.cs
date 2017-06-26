using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Responses;
using UnityEngine;

namespace HauntedCity.GameMechanics.BattleSystem
{
    
    
    [RequireComponent(typeof(Animator))]
    public class ShellBehavior : MonoBehaviour
    {

        public float DestroyTimeout = 0.01f;
        
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

        IEnumerator DestroyShell()
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Animator>().SetTrigger("Bam");
            yield return new WaitForSeconds(DestroyTimeout);
            Destroy(gameObject);
        }
        
        public IEnumerator DestroyAfterTimeout()
        {
            yield return new WaitForSeconds(Weapon.Ttl);
            StartCoroutine(DestroyShell());
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) return;
            StartCoroutine(DestroyShell());
        }


        public void Launch(Vector3 startPosition, Vector3 velocityVector)
        {
            gameObject.transform.position = startPosition;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = velocityVector * Weapon.Velocity;
        }
    }
}