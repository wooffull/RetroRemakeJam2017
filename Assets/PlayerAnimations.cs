using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    public Sprite playerStand;
    public Sprite playerCrouch;
    public Sprite playerAimUp;
    public Sprite playerJump;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    // Use this for initialization
    void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }
	
	// Update is called once per frame
	void Update () {

		if(playerMovement.isCrouch) // Pit is crouching
        {
            spriteRenderer.sprite = playerCrouch;
        }
        else if(playerMovement.isUp) // Pit is aiming up
        {
            spriteRenderer.sprite = playerAimUp;
        }
        else if(playerMovement.displacement.y != 0) // Pit is in the air
        {
            spriteRenderer.sprite = playerJump;
        }
        else if(playerMovement.displacement.y == 0 && playerMovement.displacement.x != 0) // Pit is walking
        {
            // Walking sprites go here
        }
        else if(playerMovement.displacement.y == 0 && playerMovement.displacement.x == 0) // Pit is idle
        {
            spriteRenderer.sprite = playerStand;
        }
	}
}
