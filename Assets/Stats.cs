using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public float playerDropSpeed = 2 * 1.28f;
    public float playerDropDelay = 0.5f;
    public int maxHealth = 50;
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
    private bool hasDied = false;
    private float currentTime;
    private float startTime;
    private bool isPlayerDead;
    private bool isPlayer;
    private GameObject collectible;
    private Rigidbody2D rigidBody;
    private Collider2D collider;
    private Stats playerStats;
    private AudioManager audioManager;

    // Use this for initialization
    void Start () {
        _health = maxHealth;

        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();
        isPlayerDead = false;
        playerStats = GameObject.Find("Player").GetComponent<Stats>();
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();

        // Determines if it is the player
        if (gameObject.name == "Player")
        {
            audioManager.PlayPlayerStartSound();
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
        collectible.transform.position += new Vector3(0, 0, -1); // Spawn in front of blocks
        collectible.name = "Money_" + gameObject.name;
    }

    // Method for spawning health
    void SpawnHealth()
    {
        collectible = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        collectible.transform.position = gameObject.transform.position;
        collectible.transform.position += new Vector3(0, 0, -1); // Spawn in front of blocks
    }

	// Update is called once per frame
	void Update () {
        currentTime = Time.time;

        // Determines when the player dies
        if (isPlayer && health <= 0)
        {
            Destroy(gameObject.GetComponent<PlayerMovement>());
            Destroy(gameObject.GetComponent<PlayerAnimations>());
            Destroy(gameObject.GetComponent<Shoot>());
            Destroy(rigidBody);
            if (!isPlayerDead)
            {
                startTime = Time.time;
                isPlayerDead = true;
            }

            if (currentTime > (startTime + playerDropDelay)) // Stays still for a delay before falling
            {
                if (!hasDied)
                {
                    audioManager.PlayPlayerDeathSound();
                }

                hasDied = true;
                transform.position += Vector3.down * playerDropSpeed * Time.deltaTime;
            }
        }
        else if(health <= 0) // If it is not the player, the enemy dies
        {
            // Randomly drops money or health
            if(playerStats.health != playerStats.maxHealth)
            {
                float num = Random.value;
                if(num < 0.25)
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
