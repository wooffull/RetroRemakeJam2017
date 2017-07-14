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
    private float currentTime;
    private float startTime;
    private bool isPlayerDead;
    private bool isPlayer;
    private Rigidbody2D rigidBody;
    private Collider2D collider;

    // Use this for initialization
    void Start () {
        _health = maxHealth;

        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();
        isPlayerDead = false;

        // Determines if it is the player
        if (gameObject.name == "Player")
        {
            isPlayer = true;
        }
        else
        {
            isPlayer = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        currentTime = Time.time;

        // Determines when the player dies
        if (isPlayer && health <= 0)
        {
            if(!isPlayerDead)
            {
                startTime = Time.time;
                isPlayerDead = true;
            }

            if(currentTime > (startTime + 1)) // Stays still for 1 second before falling
            {
                Destroy(rigidBody);
                Destroy(collider);
                transform.position += Vector3.down * 0.1f;
            }
        }
        else if(health <= 0) // If it is not the player, the enemy dies
        {
            Destroy(gameObject);
            // Add code to drop hearts
        }
	}

    public void Collect(Collectable c)
    {
        health += c.healthIncrease;
        money += c.moneyIncrease;
    }
}
