using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    public Sprite playerStand;
    public Sprite playerWalk1;
    public Sprite playerWalk2;
    public Sprite playerWalk3;
    public Sprite playerWalk4;
    public Sprite playerCrouch;
    public Sprite playerAimUp;
    public Sprite playerJump1;
    public Sprite playerJump2;
    public Sprite playerJump3;
    public Sprite playerJump4;
    public int animationFrameDelay = 2;

    private int frameCount = 0;
    private int startFrame = 0;
    private int currentFrame;
    private bool frameCountActive = false;
    private Sprite[] walkSprites;
    private Sprite[] jumpSprites;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    // Use this for initialization
    void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        walkSprites = new Sprite[4];
        jumpSprites = new Sprite[4];

        walkSprites[0] = playerWalk1;
        walkSprites[1] = playerWalk2;
        walkSprites[2] = playerWalk3;
        walkSprites[3] = playerWalk4;
        jumpSprites[0] = playerJump1;
        jumpSprites[1] = playerJump2;
        jumpSprites[2] = playerJump3;
        jumpSprites[3] = playerJump4; 
    }

    // Method for making the sprites face left
    void LeftSprites()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    // Method for making hte sprites face right
    void RightSprites()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
	
	// Update is called once per frame
	void Update () {

        currentFrame = Time.frameCount;

        // Keeps track of the frame count for animated movement
        if(frameCountActive && currentFrame > (startFrame + animationFrameDelay))
        {
            startFrame = Time.frameCount;
            if (frameCount == 3)
            {
                frameCount = 0;
            }
            else
            {
                frameCount++;
            }
        }

        // Determines direction the sprites will face
        if(playerMovement.isLeft)
        {
            LeftSprites();
        }
        else
        {
            RightSprites();
        }

		if(playerMovement.isCrouch) // Pit is crouching
        {
            frameCountActive = false;
            frameCount = 0;
            spriteRenderer.sprite = playerCrouch;
        }
        else if(playerMovement.isUp) // Pit is aiming up
        {
            frameCountActive = false;
            frameCount = 0;
            spriteRenderer.sprite = playerAimUp;
        }
        else if(playerMovement.displacement.y != 0) // Pit is in the air
        {
            frameCountActive = true;
            spriteRenderer.sprite = jumpSprites[frameCount];
        }
        else if(playerMovement.displacement.y == 0 && playerMovement.displacement.x != 0) // Pit is walking
        {
            frameCountActive = true;
            spriteRenderer.sprite = walkSprites[frameCount];
        }
        else if(playerMovement.displacement.y == 0 && playerMovement.displacement.x == 0) // Pit is idle
        {
            frameCountActive = false;
            frameCount = 0;
            spriteRenderer.sprite = playerStand;
        }
	}
}
