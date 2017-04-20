using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorsView : MonoBehaviour
{

    private PlayerBattleController _battleController;

    private Transform _energy;
    private Transform _health;

    private Transform _energyBar;
    private Transform _healthBar;

	// Use this for initialization
	void Start ()
	{
	    _battleController = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>().BattleController;

	    _battleController.OnEnergyChanged += UpdateEnergy;
	    _battleController.OnDamage += UpdateHealth;

	    _health = transform.Find("HealthPanel");
	    _energy = transform.Find("EnergyPanel");

	    _healthBar = _health.Find("HealthBar");
	    _energyBar = _energy.Find("EnergyBar");

	    UpdateEnergy(0);
	    UpdateHealth(0);
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
        _energy.GetComponentInChildren<Text>().text = currentEnergy + "/" + maxEnergy;

        var energyPercent = (float)currentEnergy/maxEnergy;
        _energyBar.GetComponent<Image>().fillAmount = energyPercent;
    }

    public void UpdateHealth(int delta)
    {
        var currentHealth = _battleController.BattleStats.CurrentHealth;
        var maxHealth = _battleController.BattleStats.MaxHealth;
        _health.GetComponentInChildren<Text>().text = currentHealth + "/" + maxHealth;

        var healthPercent = (float)currentHealth/maxHealth;
        _healthBar.GetComponent<Image>().fillAmount = healthPercent;

    }


}
