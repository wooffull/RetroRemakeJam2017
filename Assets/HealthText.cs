using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour {

    public int health = 0;

    private Stats playerStats;
    private Text text;

    // Use this for initialization
    void Start () {
        playerStats = GameObject.Find("Player").GetComponent<Stats>();
        text = gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        health = playerStats.health;

        text.text = health.ToString();
    }
}
