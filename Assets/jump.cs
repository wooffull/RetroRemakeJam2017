using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviours don't normally have constructors
public class jump : MonoBehaviour {

	public int jumpspeed = 500;

	private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		bool jumpIsDown = Input.GetButtonDown("Jump");
		float horizontalAxis = Input.GetAxis("Horizontal");

		if (jumpIsDown) {
			rigidBody.velocity += Vector2.up * jumpspeed * Time.deltaTime;
		}
			
		Debug.Log (jumpIsDown);
	}
}
