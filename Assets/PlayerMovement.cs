using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpHeight = 3 * 1.28f;
    public float totalJumpTime = 1.0f;
    public float walkSpeed = 3 * 1.28f;

    public bool isCrouch = false;
    public bool isUp = false;
    public bool isLeft = false;
    public Vector2 displacement;

    private Rigidbody2D rigidBody;
    private BoxCollider2D collider;

    private bool canJump = false;
    private bool isGrounded = false;
    //private bool canMove = true;
    private float jumpTimer = 0;
    private HashSet<GameObject> collidedGroundObjects;
    private AudioManager audioManager;

    // Use this for initialization
    void Start () {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<BoxCollider2D>();
        collidedGroundObjects = new HashSet<GameObject>();
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        OnCollisionStay2D(c);
    }

    void OnCollisionStay2D(Collision2D c)
    {
        if (c.contacts.Length > 0)
        {
            ContactPoint2D contact = c.contacts[0];

            // If the collision was below the player, the player is grounded
            if (Vector3.Dot(contact.normal, Vector2.up) > 0.75f)
            {
                ResetJump();
                isGrounded = true;
                collidedGroundObjects.Add(c.gameObject);
            }

            // If player hits its head, end the jump
            else if (Vector3.Dot(contact.normal, Vector2.up) < -0.75f)
            {
                if (canJump)
                {
                    audioManager.PlayDenySound();
                }

                jumpTimer = totalJumpTime;
                canJump = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        collidedGroundObjects.Remove(c.gameObject);

        // If there are no game objects directly under the player, it's no longer grounded
        if (collidedGroundObjects.Count == 0)
        {
            isGrounded = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        // Pit crouches
        if (verticalInput < 0)
        {
            isCrouch = true;
            isUp = false;
            collider.size = new Vector2(1, 1.92f * (2f / 3f));
            collider.offset = new Vector2(0, 0.95f * (2f / 3f));
            //canMove = true;
        }
        else if (verticalInput > 0) // Pit aims upward
        {
            isCrouch = false;
            isUp = true;
            collider.size = new Vector2(1, 1.92f);
            collider.offset = new Vector2(0, 0.95f);
            //canMove = false;
        }
        else // Pit is standing/jumping/falling
        {
            isCrouch = false;
            isUp = false;
            collider.size = new Vector2(1, 1.92f);
            collider.offset = new Vector2(0, 0.95f);
            //canMove = true;
        }

        // Reset movement for this tick
        displacement = Vector2.zero;

        UpdateMovement();
        UpdateJump();

        rigidBody.position += new Vector2(displacement.x, displacement.y);
    }

    private void UpdateMovement()
    {
        float walkDirection = Input.GetAxis("Horizontal");

        // Determines the direction the player is facing
        if(walkDirection < 0)
        {
            isLeft = true;
        }
        else if(walkDirection > 0)
        {
            isLeft = false;
        }

        // Add movement from walking around
        displacement.x += walkDirection * walkSpeed * Time.deltaTime;
    }

    private void UpdateJump()
    {
        bool jumpKeyDown = Input.GetButton("Jump");
        float prevJumpTimer = jumpTimer;
        float deltaTimeSinceLastJump = Time.deltaTime;

        // If no jump key is down, or the jump has finished, the player can no longer jump
        if (!jumpKeyDown || jumpTimer >= totalJumpTime)
        {
            canJump = false;
        }

        // Otherwise, if the player can jump, jump!
        else if (canJump)
        {
            // Play the jump sound if this is the beginning of the jump
            if (jumpTimer == 0)
            {
                audioManager.PlayJumpSound();
            }

            if (jumpTimer < totalJumpTime)
            {
                jumpTimer += deltaTimeSinceLastJump;

                // Do not allow the jump timer to exceed the total time allow for holding a jump
                if (jumpTimer > totalJumpTime)
                {
                    deltaTimeSinceLastJump = jumpTimer - totalJumpTime;
                    jumpTimer = totalJumpTime;
                }
            }
        }

        // If the player is jumping, prepare to move this game object up in space
        if (canJump && jumpTimer > 0)
        {
            displacement += Vector2.up * CalculateJumpIncrement(prevJumpTimer, jumpTimer);

            // Undo the gravity that was applied since the jump calculation will already be parabolic
            Vector2 currentFrameGravity = Physics2D.gravity * (jumpTimer - prevJumpTimer) * rigidBody.mass;
            rigidBody.AddForce(-currentFrameGravity, ForceMode2D.Impulse);
        }

        // Reset jump variables if grounded and can jump
        if (isGrounded && !canJump)
        {
            ResetJump();
        }
    }

    /**
     * Returns the distance the player should rise from t0 seconds to t1 seconds in the jump cycle to abide by a parabolic motion
     */
    private float CalculateJumpIncrement(float t0, float t1)
    {
        float offset0 = t0 - totalJumpTime;
        float offset1 = t1 - totalJumpTime;
        return (jumpHeight / (totalJumpTime * totalJumpTime * totalJumpTime)) * (offset1 * offset1 * offset1 - offset0 * offset0 * offset0);
    }

    private void ResetJump()
    {
        jumpTimer = 0;
        canJump = true;
    }
}
