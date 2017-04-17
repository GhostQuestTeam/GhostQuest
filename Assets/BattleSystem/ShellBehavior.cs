using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class ShellBehavior : MonoBehaviour
{
    [SerializeField] public ShellInfo ShellInfo;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        Destroy(gameObject);
    }


    public void Launch(Vector3 startPosition, Vector3 velocityVector)
    {
        gameObject.transform.position = startPosition;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = velocityVector * ShellInfo.Velocity;
    }
}