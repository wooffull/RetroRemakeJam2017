using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    public float jumpSpeed = 400;

    private bool spaceIsDown = false;
    private bool jumpEnabled = true;
    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
	}

    // Function for checking if the player is on the ground
    /// ???????? HELP
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, -Vector2.up);

        if(hit.collider != null)
        {
            float distance = Mathf.Abs(hit.point.y - gameObject.transform.position.y);

            if(distance > 0.1) // Checks if the player is 0.1 above an object
            {
                return false;
            }
            return true;
        }
        return false; // Unreachable
    }
	
	// Update is called once per frame
	void Update () {
        spaceIsDown = Input.GetButtonDown("Jump");
        Debug.Log(rigidBody.velocity);
        Debug.Log(Time.deltaTime);
        
        // Checks if the jump key is pressed and if the player can jump
        if(spaceIsDown && jumpEnabled)
        {
            rigidBody.velocity += Vector2.up * jumpSpeed * Time.deltaTime;
            jumpEnabled = false;
        }

        // Allows the player to jump again when the player has no vertical velocity
        if(IsGrounded())
        {
            jumpEnabled = true;
        }
	}
}
