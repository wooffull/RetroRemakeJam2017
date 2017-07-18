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
    public Sprite playerGroundShoot1;
    public Sprite playerGroundShoot2;
    public Sprite playerGroundShoot3;
    public Sprite playerGroundShoot4;
    public Sprite playerAirShoot1;
    public Sprite playerAirShoot2;
    public Sprite playerAirShoot3;
    public Sprite playerAirShoot4;
    public Sprite playerUpShoot1;
    public Sprite playerUpShoot2;
    public Sprite playerUpShoot3;
    public Sprite playerUpShoot4;
    public int animationFrameDelay = 2;
    public int fastAnimationFrameDelay = 1;
    public int fastAnimationEndFrameDuration = 3;

    private int frameCount = 0;
    private int fastFrameCount = 0;
    private int startFrame = 0;
    private int fastStartFrame = 0;
    private int currentFrame;
    private bool frameCountActive = false;
    private bool fastFrameCountActive = false;
    private bool shootActive = false;
    private bool endShootAnimation = false;
    private Sprite[] walkSprites;
    private Sprite[] jumpSprites;
    private Sprite[] groundShootSprites;
    private Sprite[] airShootSprites;
    private Sprite[] upShootSprites;
    private Sprite[] activeShootAnimation;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    // Use this for initialization
    void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        walkSprites = new Sprite[4];
        jumpSprites = new Sprite[4];
        groundShootSprites = new Sprite[4];
        airShootSprites = new Sprite[4];
        upShootSprites = new Sprite[4];
        activeShootAnimation = new Sprite[4];

        walkSprites[0] = playerWalk1;
        walkSprites[1] = playerWalk2;
        walkSprites[2] = playerWalk3;
        walkSprites[3] = playerWalk4;
        jumpSprites[0] = playerJump1;
        jumpSprites[1] = playerJump2;
        jumpSprites[2] = playerJump3;
        jumpSprites[3] = playerJump4;
        groundShootSprites[0] = playerGroundShoot1;
        groundShootSprites[1] = playerGroundShoot2;
        groundShootSprites[2] = playerGroundShoot3;
        groundShootSprites[3] = playerGroundShoot4;
        airShootSprites[0] = playerAirShoot1;
        airShootSprites[1] = playerAirShoot2;
        airShootSprites[2] = playerAirShoot3;
        airShootSprites[3] = playerAirShoot4;
        upShootSprites[0] = playerUpShoot1;
        upShootSprites[1] = playerUpShoot2;
        upShootSprites[2] = playerUpShoot3;
        upShootSprites[3] = playerUpShoot4;
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

        // Animation for shooting
        if(shootActive)
        {
            spriteRenderer.sprite = activeShootAnimation[fastFrameCount];

            // Goes through the animation
            if(!endShootAnimation)
            {
                if (fastFrameCountActive && currentFrame > (fastStartFrame + fastAnimationFrameDelay))
                {
                    fastStartFrame = Time.frameCount;
                    if (fastFrameCount == 3) // End the while loop when the animation finishes
                    {
                        endShootAnimation = true;
                    }
                    else
                    {
                        fastFrameCount++;
                    }
                }
            }
            else // Delays on the end frame to make the animation transition look smoother
            {
                if (fastFrameCountActive && currentFrame > (fastStartFrame + fastAnimationEndFrameDuration))
                {
                    shootActive = false;
                    endShootAnimation = false;
                }
            }
        }

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
        if (playerMovement.isLeft)
        {
            LeftSprites();
        }
        else
        {
            RightSprites();
        }

        // Prevents the animations of shooting from being overriden
        if(!shootActive)
        {
            if (playerMovement.isCrouch) // Pit is crouching
            {
                frameCountActive = false;
                fastFrameCountActive = false;
                frameCount = 0;
                fastFrameCount = 0;
                spriteRenderer.sprite = playerCrouch;
            }
            else if (Input.GetButtonDown("Fire1") && playerMovement.isUp) // Pit is shooting up
            {
                frameCountActive = false;
                fastFrameCountActive = true;
                frameCount = 0;
                shootActive = true;
                activeShootAnimation = upShootSprites;
            }
            else if (playerMovement.isUp) // Pit is aiming up
            {
                frameCountActive = false;
                fastFrameCountActive = false;
                frameCount = 0;
                fastFrameCount = 0;
                spriteRenderer.sprite = playerAimUp;
            }
            else if (Input.GetButtonDown("Fire1") && playerMovement.displacement.y != 0) // Pit is shooting in the air
            {
                frameCountActive = false;
                fastFrameCountActive = true;
                frameCount = 0;
                shootActive = true;
                activeShootAnimation = airShootSprites;
            }
            else if (Input.GetButtonDown("Fire1")) // Pit is shooting
            {
                frameCountActive = false;
                fastFrameCountActive = true;
                frameCount = 0;
                shootActive = true;
                activeShootAnimation = groundShootSprites;
            }
            else if (playerMovement.displacement.y != 0) // Pit is in the air
            {
                frameCountActive = true;
                fastFrameCountActive = false;
                fastFrameCount = 0;
                spriteRenderer.sprite = jumpSprites[frameCount];
            }
            else if (playerMovement.displacement.y == 0 && playerMovement.displacement.x != 0) // Pit is walking
            {
                frameCountActive = true;
                fastFrameCountActive = false;
                fastFrameCount = 0;
                spriteRenderer.sprite = walkSprites[frameCount];
            }
            else if (playerMovement.displacement.y == 0 && playerMovement.displacement.x == 0) // Pit is idle
            {
                frameCountActive = false;
                fastFrameCountActive = false;
                frameCount = 0;
                fastFrameCount = 0;
                spriteRenderer.sprite = playerStand;
            }
        }
	}
}
