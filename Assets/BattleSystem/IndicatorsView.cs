using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorsView : MonoBehaviour
{

    private PlayerBattleController _battleController;

    private Text _health;
    private Text _energy;
	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(_wait());//TODO Найти лучший способ ожидания инициализации PlayerBattleBehavior
	    _battleController = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>().BattleController;

	    _battleController.OnEnergyChanged += UpdateEnergy;
	    _battleController.OnDamage += UpdateHealth;

	    _health = transform.Find("Health").GetComponent<Text>();
	    _energy = transform.Find("Energy").GetComponent<Text>();
	}

    private IEnumerator _wait()
    {
        yield return new WaitForSeconds(1f);
    }
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        _battleController.OnEnergyChanged -= UpdateEnergy;
        _battleController.OnDamage -= UpdateHealth;
    }

    public void UpdateEnergy(int delta)
    {
        var currentEnergy = _battleController.BattleStats.CurrentEnergy;
        var maxEnergy = _battleController.BattleStats.MaxEnergy;
        _energy.text = currentEnergy + "/" + maxEnergy;
    }

    public void UpdateHealth(int delta)
    {
        var currentHealth = _battleController.BattleStats.CurrentHealth;
        var maxHealth = _battleController.BattleStats.MaxHealth;
        _health.text = currentHealth + "/" + maxHealth;
    }


}
