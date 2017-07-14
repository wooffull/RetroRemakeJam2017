using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Some simple code for One Way (cloud) Platforms.
 * USAGE: works with horizontal platforms that only the player will stand on.
 * Platform pivot should be where the player can stand. Player pivot should be at the player's feet.
 * <3 - @x01010111
 * Credit - https://pastebin.com/KArtkV1E
 */
public class JumpThrough : MonoBehaviour {

    public string playerName = "Player";
    private GameObject player;

    // Use this for initialization
    void Start () {
        //Find player by name
        player = GameObject.Find(playerName);
        if (player == null) Debug.LogError("(One Way Platform) Please enter correct player name in Inspector for: " + gameObject.name);
    }
	
	// Update is called once per frame
	void Update () {
        //Check to see if player is under the platform. Collide only if the player is above the platform.
        if (player != null)
        {
            if (player.transform.position.y < this.transform.position.y) gameObject.GetComponent<Collider2D>().enabled = false;
            else gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }
}
