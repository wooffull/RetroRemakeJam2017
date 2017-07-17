using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    public int score = 0;

    private Stats playerStats;
    private Text text;

	// Use this for initialization
	void Start () {
        playerStats = GameObject.Find("Player").GetComponent<Stats>();
        text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        score = playerStats.money;

        string displayScore = score.ToString().PadLeft(3, '0');
        text.text = displayScore;
	}
}
