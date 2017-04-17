using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public EnemyBattleController BattleController { get; private set; }
    public EnemyBattleStats BattleStats;
	// Use this for initialization
	void Start ()
	{
	    BattleStats.ResetHealth();
		BattleController = new EnemyBattleController(BattleStats);
	    BattleController.OnDeath += () => Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnCollisionEnter(Collision collision)
    {
        var shell = collision.gameObject.GetComponent<ShellBehavior>();
        if(shell == null) return;
        BattleController.TakeDamage(shell.ShellInfo);
    }

}
