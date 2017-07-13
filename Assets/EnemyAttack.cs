using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    private GameObject player = GameObject.Find("Player");
    private Collider2D playerCollider;
    private Collider2D enemyCollider;
    private Stats playerStats;
    private Stats enemyStats;
    private Invincibility playerInvincibility;

	// Use this for initialization
	void Start () {
        playerCollider = player.GetComponent<Collider2D>();
        enemyCollider = gameObject.GetComponent<Collider2D>();
        playerStats = player.GetComponent<Stats>();
        enemyStats = gameObject.GetComponent<Stats>();
        playerInvincibility = player.GetComponent<Invincibility>();
	}
	
	// Update is called once per frame
	void Update () {

        // Damages the player and causes the player to turn invincible
		if(playerCollider.IsTouching(enemyCollider) && playerInvincibility.isInvincible == false)
        {
            playerStats.health -= enemyStats.damage;
            playerInvincibility.MakeInvincible();
        }
        Debug.Log(playerStats.health);
	}
}
