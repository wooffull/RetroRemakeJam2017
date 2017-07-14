using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    private GameObject player;
    private GameObject[] enemies;
    private Collider2D playerCollider;
    private Collider2D enemyCollider;
    private Stats playerStats;
    private Stats enemyStats;
    private Invincibility playerInvincibility;
    private AudioManager audioManager;
    private Queue<GameObject> collisionOffQueue;
    private GameObject currentEnemy;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        playerCollider = player.GetComponent<Collider2D>();
        enemyCollider = gameObject.GetComponent<Collider2D>();
        playerStats = player.GetComponent<Stats>();
        enemyStats = gameObject.GetComponent<Stats>();
        playerInvincibility = player.GetComponent<Invincibility>();
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();

        // Makes the enemies not collide with each other
        collisionOffQueue = new Queue<GameObject>();
        foreach(GameObject enemy in enemies)
        {
            collisionOffQueue.Enqueue(enemy);
        }

        while(collisionOffQueue.Count != 0)
        {
            currentEnemy = collisionOffQueue.Dequeue();
            Collider2D currentEnemyCollider = currentEnemy.GetComponent<Collider2D>();
            foreach(GameObject enemy in collisionOffQueue)
            {
                Physics2D.IgnoreCollision(currentEnemyCollider, enemy.GetComponent<Collider2D>());
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        // Damages the player and causes the player to turn invincible
		if(playerCollider.IsTouching(enemyCollider) && playerInvincibility.isInvincible == false)
        {
            playerStats.health -= enemyStats.damage;
            playerInvincibility.MakeInvincible();

            audioManager.PlayTakeDamageSound();
        }
	}
}
