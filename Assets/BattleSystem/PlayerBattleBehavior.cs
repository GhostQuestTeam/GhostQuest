using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class PlayerBattleBehavior : MonoBehaviour, IShooter
{
    public PlayerBattleController BattleController;// { get; private set; }

    private ShellFactory _shellFactory;


	void Awake () {
	    var solidity = new Solidity(100,0);
	    var weapons = new[] {new WeaponInfo("sphere", 3, 0)};

	    _shellFactory = new ShellFactory();
	    var battleStats = new PlayerBattleStats(solidity, 100, 1, weapons);
		BattleController = new PlayerBattleController(battleStats, this);

	    StartCoroutine(Regenerate());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot(WeaponInfo weapon)
    {
        GameObject shell = _shellFactory.CreateShell(weapon);
        var cameraForward = GetComponentInChildren<Camera>().transform.forward;
        var startPosition = transform.position + cameraForward * 2;
        shell.GetComponent<ShellBehavior>().Launch(startPosition, cameraForward);
    }

    public IEnumerator Regenerate()
    {
        while (true)
        {
            BattleController.Regenerate();
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnShootClick()
    {
        BattleController.TryShoot();
    }
}
