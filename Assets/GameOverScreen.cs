using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {

    public float totalTimeUntilNextScene = 3.0f;

    private float timer;

	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer >= totalTimeUntilNextScene)
        {
            SceneManager.LoadScene("GameScene");
        }
	}
}
