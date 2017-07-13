using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public int healthIncrease = 0;
    public int moneyIncrease = 0;

    private GameObject player;
    private Stats playerStats;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<Stats>();
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == player)
        {
            playerStats.Collect(this);
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
