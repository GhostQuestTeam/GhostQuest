using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class PlayerBattleBehavior : MonoBehaviour, IShooter, IWeaponSpeedController
{
    public PlayerBattleController BattleController;// { get; private set; }

    private ShellFactory _shellFactory;
    public string[] WeaponIDs;

    private Dictionary<string, bool> _blockedWeapons;

	void Awake () {
	    var solidity = new Solidity(100,0);
	    var weapons = new WeaponInfo[WeaponIDs.Length];
	    _blockedWeapons = new Dictionary<string, bool>();
	    for (var i = 0; i < WeaponIDs.Length; i++)
	    {

	        weapons[i] = WeaponLoader.LoadWeapon(WeaponIDs[i]);
	        _blockedWeapons.Add(weapons[i].Id, false);
	    }

	    _shellFactory = new ShellFactory();
	    var battleStats = new PlayerBattleStats(solidity, 100, 1, weapons);
		BattleController = new PlayerBattleController(battleStats, this, this);

	    StartCoroutine(Regenerate());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot(WeaponInfo weapon)
    {
        GameObject shell = _shellFactory.CreateShell(weapon);
        var cameraForward = GetComponentInChildren<Camera>().transform.forward;
        var startPosition = transform.position + cameraForward * 1;
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


    private IEnumerator _blockWeaponCoroutine(float sec, string weapontId)
    {
        _blockedWeapons[weapontId] = true;
        yield return new WaitForSeconds(sec);
        _blockedWeapons[weapontId] = false;
    }

    public bool CanShoot(WeaponInfo weapon)
    {
        return !_blockedWeapons[weapon.Id];
    }

    public void BlockWeapon(WeaponInfo weapon, float duration)
    {
        StartCoroutine(_blockWeaponCoroutine(duration, weapon.Id));
    }
}
