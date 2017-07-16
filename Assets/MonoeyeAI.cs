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
	private float xTarget;
	private float xTargetLeft;
	private float xTargetRight;
	private float viewportMax;
	private float viewportMin;
	private GameObject player;
	private int timesFlipped;
	private float playerWidth;
	private RectTransform playerRect;
	private float xMax;
	private float xMin;
	private float yVelMax;
	private float xVelMax;
	private float yTargetForce;
	private float xTargetForce;
	private float yAccMax;
	private float yAccMin;
	private float xAccMax;
	private float xAccMin;
	private float yTargetTop;
	private float yTargetBot;
	private bool enteredScreen;
	private float viewportTop;
	private float viewportBot;
	private bool seeking;
	// Use this for initialization
	void Start () {
		seeking = false;
		player = GameObject.Find ("Player");
		body = GetComponent<Rigidbody2D> ();
		timesFlipped = 0;
		camera = Camera.main;
		viewportTop = camera.ViewportToWorldPoint(new Vector3(0, 1f, transform.position.z)).y;
		viewportBot = camera.ViewportToWorldPoint(new Vector3(0, 0f, transform.position.z)).y;
		yTargetTop = camera.ViewportToWorldPoint(new Vector3(0, 0.85f, transform.position.z)).y;
		yTargetBot = camera.ViewportToWorldPoint(new Vector3(0, 0.75f, transform.position.z)).y;
		viewportMin = camera.ViewportToWorldPoint (new Vector3 (0, 0, transform.position.z)).x;
		viewportMax = camera.ViewportToWorldPoint (new Vector3 (1f, 0, transform.position.z)).x;
		viewportLeft = camera.ViewportToWorldPoint (new Vector3 (0.01f, 0, transform.position.z)).x;
		viewportRight = camera.ViewportToWorldPoint (new Vector3 (0.99f, 0, transform.position.z)).x;
		xTargetLeft = camera.ViewportToWorldPoint (new Vector3 (0.15f, 0, transform.position.z)).x;
		xTargetRight = camera.ViewportToWorldPoint (new Vector3 (0.85f, 0, transform.position.z)).x;
//		xTargetLeft = viewportLeft;
//		xTargetRight = viewportRight;
		yAccMax = 3f;
		yAccMin = 1f;
		yVelMax = 0.1f;
		yTarget = yTargetBot;

		xAccMax = 5f;
		xAccMin = 0.5f;
		xVelMax = 0.01f;
		//get the collider 2d player and then get the width 
		xSpeed = 0.05f;
		if (player.transform.position.x - transform.position.x >= 0) {
			dir = 1;
			xTarget = xTargetRight;
		} else {
			dir = -1;
			xTarget = xTargetLeft;
		}
		if (transform.position.y < viewportTop) {
			enteredScreen = true;
		} else {
			enteredScreen = false;
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
		if (seeking) {
			xTarget = player.transform.position.x;
		}
		if (seeking) {
			yTarget = viewportBot;
		}
		yTargetTop = camera.ViewportToWorldPoint(new Vector3(0, 0.85f, transform.position.z)).y;
		yTargetBot = camera.ViewportToWorldPoint(new Vector3(0, 0.75f, transform.position.z)).y;
		viewportTop = camera.ViewportToWorldPoint(new Vector3(0, 1f, transform.position.z)).y;
		viewportBot = camera.ViewportToWorldPoint(new Vector3(0, 0f, transform.position.z)).y;
		// Set y target
//		yTarget = camera.ViewportToWorldPoint (new Vector3 (0, 0.8f, transform.position.z)).y;
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

		if (!enteredScreen && transform.position.y < viewportTop) {
			enteredScreen = true;
		}
		if (enteredScreen && transform.position.y > viewportTop) {
			transform.position = new Vector3 (transform.position.x, viewportTop);
		}

		if (transform.position.y < yTarget) {
			if (!seeking) {
				yTarget = yTargetTop;
			}
		} else if (transform.position.y > yTarget) {
			if (!seeking) {
				yTarget = yTargetBot;
			}
		}
//		if (dir == -1) {
//			xTarget = xTargetLeft;
//		} else {
//			xTarget = xTargetRight;
//		}

		xTargetForce = (xTarget - transform.position.x) * 10;
		if (Mathf.Abs(xTargetForce) > xAccMax) {
			xTargetForce = xAccMax * Mathf.Sign(xTargetForce);
		}
		if (Mathf.Abs(xTargetForce) < xAccMin) {
			xTargetForce = xAccMin * Mathf.Sign(xTargetForce);
		}
		body.AddForce (new Vector2 (xTargetForce, 0));
		// Limit my velocity
		if (Mathf.Abs(body.velocity.x) > xVelMax) {
			float xval;
			// Add an opposite force represented by difference between yVel and yVelMax
			xval = -(body.velocity.x - (xVelMax * Mathf.Sign(body.velocity.x)));

			body.AddForce (new Vector2(xval, 0));
		}

		if (transform.position.x >= xTarget && dir == 1) {
			ChangeDir (-1);
			if (!seeking) {
				xTarget = xTargetLeft;
			}
		} else if (transform.position.x <= xTarget && dir == -1) {
			ChangeDir (1);
			if (!seeking) {
				xTarget = xTargetRight;
			}
		}
		if (transform.position.x > viewportRight) {
			transform.position = new Vector3(viewportRight, transform.position.y);
		}
		if (transform.position.x < viewportLeft) {
			transform.position = new Vector3(viewportLeft, transform.position.y);
		}
//		Debug.Log (transform.position.x);
//		Debug.Log (xTarget);
//		Debug.Log (yTarget);





//		// Move player horiz
//		transform.position += new Vector3 (xSpeed * dir, 0);
//		// Flip direction when near edge of screen
//		if (transform.position.x >= viewportRight) {
//			ChangeDir (-1);
//		} else if (transform.position.x <= viewportLeft) {
//			ChangeDir (1);
//		}
//		// Limit horiz movespeed
//		if (xSpeed > 0.05f) {
//			xSpeed = 0.05f;
//		}
	}

	void ChangeDir (int newDir) {
		dir = newDir;
		timesFlipped++;
	}

	void OnMonoeyeSeek () {
		if (timesFlipped > 3) {
			seeking = true;
		}
	}
}
