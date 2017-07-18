using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour {

    private int highScore;
    private Collider2D collider;
    private Collider2D playerCollider;
    private GameObject player;
    private Stats playerStats;

	// Use this for initialization
	void Start () {
        highScore = PlayerPrefs.GetInt("High Score");
        collider = gameObject.GetComponent<Collider2D>();
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<Collider2D>();
        playerStats = player.GetComponent<Stats>();
	}
	
	// Update is called once per frame
	void Update () {
		if(collider.IsTouching(playerCollider))
        {
            // Set a new high score
            if (playerStats.money > highScore)
            {
                PlayerPrefs.SetInt("High Score", playerStats.money);
            }

            PlayerPrefs.SetInt("Current Score", playerStats.money);
            SceneManager.LoadScene("CompleteScene");
        }
	}
}
