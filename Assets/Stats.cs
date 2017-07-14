﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public int maxHealth = 100;
    public int money = 0;
    public int damage = 10;
    public GameObject moneyPrefab;
    public GameObject healthPrefab;

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
    private GameObject collectible;
    private Rigidbody2D rigidBody;
    private Collider2D collider;
    private Stats playerStats;

    // Use this for initialization
    void Start () {
        _health = maxHealth;

        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();
        isPlayerDead = false;
        playerStats = GameObject.Find("Player").GetComponent<Stats>();

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
	
    // Method for spawning money
    void SpawnMoney()
    {
        collectible = Instantiate(moneyPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        collectible.transform.position = gameObject.transform.position;
    }

    // Method for spawning health
    void SpawnHealth()
    {
        collectible = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        collectible.transform.position = gameObject.transform.position;
    }

	// Update is called once per frame
	void Update () {
        currentTime = Time.time;

        // Determines when the player dies
        if (isPlayer && health <= 0)
        {
            Destroy(gameObject.GetComponent<PlayerMovement>());
            Destroy(rigidBody);
            if (!isPlayerDead)
            {
                startTime = Time.time;
                isPlayerDead = true;
            }

            if(currentTime > (startTime + 1)) // Stays still for 1 second before falling
            {
                transform.position += Vector3.down * 0.1f;
            }
        }
        else if(health <= 0) // If it is not the player, the enemy dies
        {
            // Randomly drops money or health
            if(playerStats.health != playerStats.maxHealth)
            {
                float num = Random.value;
                if(num < 0.5)
                {
                    SpawnHealth();
                }
                else
                {
                    SpawnMoney();
                }
            }
            else // Creates a money object when dying - Pit is at full health
            {
                SpawnMoney();
            }

            Destroy(gameObject);
        }
	}

    public void Collect(Collectable c)
    {
        health += c.healthIncrease;
        money += c.moneyIncrease;
    }
}
