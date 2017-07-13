using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public int maxHealth = 100;
    public int money = 0;
    public int damage = 10;

    public int health
    {
        get { return _health; }
        set
        {
            _health = value;

            if (_health > maxHealth)
            {
                _health = maxHealth;
            }
        }
    }

    private int _health;

	// Use this for initialization
	void Start () {
        _health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Collect(Collectable c)
    {
        health += c.healthIncrease;
        money += c.moneyIncrease;
    }
}
