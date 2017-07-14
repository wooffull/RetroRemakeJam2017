using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public int healthIncrease = 0;
    public int moneyIncrease = 0;
    public float duration = 10;
    public float warningDuration = 3;
    public float warningChangeTime = 0.25f;
    public bool lastForever = false;

    private float currentTime;
    private float startTime;
    private float currentWarningChangeTime;
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private Stats playerStats;
    private AudioManager audioManager;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<Stats>();
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        startTime = Time.time;
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

    // Method for toggling sprite visibility on and off
    void ToggleVisbility()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
        currentTime = Time.time;

        if(!lastForever)
        {
            // Reaches the time where it warns the player of dissapearing
            if (currentTime >= (startTime + (duration - warningDuration)))
            {
                // Toggles visibility
                if (currentTime > (warningChangeTime + currentWarningChangeTime))
                {
                    ToggleVisbility();
                    currentWarningChangeTime = Time.time;
                }
            }

            // Reaches the time where it dissapears
            if(currentTime >= (startTime + duration))
            {
                Destroy(gameObject);
            }
        }
	}
}
