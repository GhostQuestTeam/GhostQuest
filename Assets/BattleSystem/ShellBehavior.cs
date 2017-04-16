using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class ShellBehavior : MonoBehaviour
{

    [SerializeField]
    public ShellInfo ShellInfo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Launch(Vector3 startPosition)
    {
        gameObject.transform.position = startPosition;
        Rigidbody rb = GetComponent<Rigidbody> ();
        startPosition.Normalize();
        rb.velocity = startPosition * ShellInfo.Velocity;
    }
}
