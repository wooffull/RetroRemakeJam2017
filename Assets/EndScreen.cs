using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour {

    private int highScore;
    private int currentScore;
    private Text highScoreText;
    private Text currentScoreText;

    // Use this for initialization
    void Start ()
    {
        highScore = PlayerPrefs.GetInt("High Score");
        highScoreText = GameObject.Find("High Score").GetComponent<Text>();
        currentScoreText = GameObject.Find("Player Score").GetComponent<Text>();
        currentScore = PlayerPrefs.GetInt("Current Score");
    }
	
	// Update is called once per frame
	void Update ()
    {
        highScoreText.text = "High Score: " + highScore.ToString().PadLeft(3, '0');
        currentScoreText.text = "Your Score: " + currentScore.ToString().PadLeft(3, '0');

        // Quit the game
        if (Input.GetKeyDown("q"))
        {
            Application.Quit();
        }
        else if (Input.anyKeyDown) // Play again
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
