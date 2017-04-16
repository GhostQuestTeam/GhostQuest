using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class PlayerBattleBehavior : MonoBehaviour, IShooter
{

    private PlayerBattleStats _battleStats;
    private PlayerBattleController _battleController;

    private ShellFactory _shellFactory;
	// Use this for initialization
	void Start () {


	    var solidity = new Solidity(100,0);
	    var weapons = new[] {new WeaponInfo("sphere", 3, 0)};

	    _shellFactory = new ShellFactory();
	    _battleStats = new PlayerBattleStats(solidity, 100, 1, weapons);
		_battleController = new PlayerBattleController(_battleStats, this);
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

    public void OnShootClick()
    {
        _battleController.TryShoot();
    }
}
