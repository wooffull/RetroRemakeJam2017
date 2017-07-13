using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour {

    public float walkSpeed = 6;

    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float walk = Input.GetAxis("Horizontal");

        // Inputs left - Pit moves left
        if(walk < 0)
        {
            rigidBody.position += Vector2.left * (walkSpeed / 100);
        }
        else if(walk > 0) // Inputs right - Pit moves right
        {
            rigidBody.position += Vector2.right * (walkSpeed / 100);
        }
	}
}
