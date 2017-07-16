using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoeyeAI : MonoBehaviour {

	private float intermittent_pos;
	private Collider2D collid;
	private new Camera camera;
	private float yTarget;
	public float xSpeed;
	private int dir;
	private Rigidbody2D body;
	private float viewportLeft;
	private float viewportRight;
	private float viewportLeftInner;
	private float viewportRightInner;
	private float viewportMax;
	private float viewportMin;
	private GameObject player;
	private int timesFlipped;
	private float playerWidth;
	private RectTransform playerRect;
	private float xMax;
	private float xMin;
	private float yVelMax;
	private float yTargetForce;
	private float yAccMax;
	private float yAccMin;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		body = GetComponent<Rigidbody2D> ();
		collid = gameObject.GetComponent<Collider2D> ();
		timesFlipped = 0;
		camera = Camera.main;
		yTarget = camera.ViewportToWorldPoint(new Vector3(0, 0.8f, transform.position.z)).y;
		viewportMin = camera.ViewportToWorldPoint (new Vector3 (0, 0, transform.position.z)).x;
		viewportMax = camera.ViewportToWorldPoint (new Vector3 (1f, 0, transform.position.z)).x;
		viewportLeft = camera.ViewportToWorldPoint (new Vector3 (0.1f, 0, transform.position.z)).x;
		viewportRight = camera.ViewportToWorldPoint (new Vector3 (0.9f, 0, transform.position.z)).x;
		viewportLeftInner = camera.ViewportToWorldPoint (new Vector3 (0.3f, 0, transform.position.z)).x;
		viewportRightInner = camera.ViewportToWorldPoint (new Vector3 (0.7f, 0, transform.position.z)).x;
		yAccMax = 10f;
		yAccMin = 0.5f;
		yVelMax = 1.0f;
		//get the collider 2d player and then get the width 
		xSpeed = 0.05f;
		if (player.transform.position.x - transform.position.x >= 0) {
			dir = 1;
		} else {
			dir = -1;
		}
	}
	
	// Update is called once per frame
	void Update () {


		// On start, move to desired screenpos and wait
		// Continue to oscillate around that screenheight
		// At screenheight, move left to right, <screenmin screenmax>
		// After 4 turns, seek player if x is close enough to player x
		// set oscillation min max to <player + const, player - const>

	}

	void FixedUpdate () {
		// Set y target
		yTarget = camera.ViewportToWorldPoint (new Vector3 (0, 0.8f, transform.position.z)).y;
		// Set the seek force to target
		yTargetForce = (yTarget - transform.position.y) * 15;
		// Limit the seek force min/max
		if (Mathf.Abs(yTargetForce) > yAccMax) {
			yTargetForce = yAccMax * Mathf.Sign(yTargetForce);
		}
		if (Mathf.Abs(yTargetForce) < yAccMin) {
			yTargetForce = yAccMin * Mathf.Sign(yTargetForce);
		}
		body.AddForce (new Vector2 (0, yTargetForce));
		// Limit my velocity
		if (Mathf.Abs(body.velocity.y) > yVelMax) {
			float yval;
			// Add an opposite force represented by difference between yVel and yVelMax
			yval = -(body.velocity.y - (yVelMax * Mathf.Sign(body.velocity.y)));

			body.AddForce (new Vector2(0, yval));
		}

		// Move player horiz
		transform.position += new Vector3 (xSpeed * dir, 0);
		// Flip direction when near edge of screen
		if (transform.position.x >= viewportRight) {
			ChangeDir (-1);
		} else if (transform.position.x <= viewportLeft) {
			ChangeDir (1);
		}
		// Limit horiz movespeed
		if (xSpeed > 0.05f) {
			xSpeed = 0.05f;
		}
	}

	void ChangeDir (int newDir) {
		dir = newDir;
		timesFlipped++;
	}
}
