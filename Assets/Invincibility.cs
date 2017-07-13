using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour {

    public float invincibilityTime = 3;
    public float spriteColorChangeTime = 0.025f;
    public bool isInvincible = false;

    private float currentTime;
    private float startTime;
    private float colorChangeTime;
    private int enemyCount;
    private bool isColliding;
    private Stats playerStats;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private GameObject[] enemies;
    private Collider2D[] enemyColliders;

	// Use this for initialization
	void Start () {
        playerCollider = gameObject.GetComponent<Collider2D>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerStats = GameObject.Find("Player").GetComponent<Stats>();

        enemyCount = enemies.Length;
        enemyColliders = new Collider2D[enemyCount];
        for(int i = 0; i < enemyCount; i++)
        {
            enemyColliders[i] = enemies[i].GetComponent<Collider2D>();
        }
	}

    // Method for making the object invincible
    public void MakeInvincible()
    {
        isInvincible = true;
        startTime = Time.time;
        colorChangeTime = Time.time;
    }

    // Method for toggling collision off between the player and enemies
    public void ToggleCollisionOff()
    {
        for(int i = 0; i < enemyCount; i++)
        {
            Physics2D.IgnoreCollision(playerCollider, enemyColliders[i], true);
        }
    }

    // Method for toggling collision on between the player and enemies
    public void ToggleCollisionOn()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Physics2D.IgnoreCollision(playerCollider, enemyColliders[i], false);
        }
    }

    // Method for making the player's sprite toggle colors
    public void ToggleColor()
    {
        if (spriteRenderer.color == Color.white)
        {
            spriteRenderer.color = Color.blue;
        }
        else if (spriteRenderer.color == Color.blue)
        {
            spriteRenderer.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update () {
        currentTime = Time.time;

        // If the invincibility time has passed, set invincibility off
		if(isInvincible && currentTime > (startTime + invincibilityTime))
        {
            isInvincible = false;
            ToggleCollisionOn();
            spriteRenderer.color = Color.white;
        }

        // Lets the player pass through enemies while invincible
        if(isInvincible)
        {
            ToggleCollisionOff();

            // Toggles color to resemble invinicibility
            if(currentTime > (colorChangeTime + spriteColorChangeTime) && playerStats.health > 0)
            {
                ToggleColor();
                colorChangeTime = Time.time;
            }
        }
	}
}
