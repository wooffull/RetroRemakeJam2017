using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    private int highScore;
    private Text text;

    // Use this for initialization
    void Start()
    {
        highScore = PlayerPrefs.GetInt("High Score");
        text = GameObject.Find("High Score").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {

        text.text = "High Score: " + highScore.ToString().PadLeft(3, '0');

        if (Input.GetKeyDown("q"))
        {
            Application.Quit();
        }
        else if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("GameScene");
        }
	}
}
