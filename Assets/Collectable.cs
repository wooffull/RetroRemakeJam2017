using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public int healthIncrease = 0;
    public int moneyIncrease = 0;

    private GameObject player;
    private Stats playerStats;
    private AudioManager audioManager;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<Stats>();
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == player)
        {
            if (healthIncrease > 0)
            {
                audioManager.PlayHealSound();
            }

            playerStats.Collect(this);
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
