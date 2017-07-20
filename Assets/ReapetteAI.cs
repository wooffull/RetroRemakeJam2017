using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReapetteAI : MonoBehaviour {

	private new Camera camera;
	private Rigidbody2D body;
	private GameObject player;
	private int dir;
	private int timesFlipped;
	private int timesFlippedMax;
	private float viewportLeft;
	private float viewportRight;
	private float viewportTop;
	private float viewportBot;
	private float xTarget;
	private float xTargetLeft;
	private float xTargetRight;
	private float yTarget;
	private float yTargetTop;
	private float yTargetBot;
	private float ySeekSlice;
	private float ySeekSliceBot;
	private float yVelMax;
	private float xVelMax;
	private float xTargetForce;
	private float yTargetForce;
	private float yAccMax;
	private float yAccMin;
	private float xAccMax;
	private float xAccMin;
	private float ySeekOffset;
	private float xSeekMax;
	private float xSeekMin;
	private float xSeekSlice;
	private bool seeking;
	private bool seekingPit;
	private bool enteredScreen;
	private bool soughtOnce;
	private bool xSoughtOnce;
	private bool ySoughtOnce;
	private float timeSinceXMet;
	private float timeSinceYMet;
	private bool xTargetMet;
	private bool yTargetMet;
	private float height;
	// Use this for initialization
	void Start () {
		
		timeSinceXMet = 0;
		timeSinceYMet = 0;
		// Has seekZone collider hit player? (Broad phase seek)
		seeking = false;
		// Am I on my final descent, attempting to attack player? (Narrow phase seek)
		seekingPit = false;
		soughtOnce = false;
		xSoughtOnce = false;
		ySoughtOnce = false;
		player = GameObject.Find ("Player");
		body = GetComponent<Rigidbody2D> ();
		// How many times have I reached left/right side of screen?
		timesFlipped = 0;
		timesFlippedMax = 4;
		camera = Camera.main;
		// Current world point top of screen
		viewportTop = camera.ViewportToWorldPoint(new Vector3(0, 1f, transform.position.z)).y;
		// Current world point bottom of screen
		viewportBot = camera.ViewportToWorldPoint(new Vector3(0, 0f, transform.position.z)).y;
		// Current world point representing upper wander vertical seek target
		yTargetTop = camera.ViewportToWorldPoint(new Vector3(0, 0.85f, transform.position.z)).y;
		// Current world point representing lower wander vertical seek target
		yTargetBot = camera.ViewportToWorldPoint(new Vector3(0, 0.75f, transform.position.z)).y;
		// Permanent world point representing left side of screen
		viewportLeft = camera.ViewportToWorldPoint (new Vector3 (0.01f, 0, transform.position.z)).x;
		// Permanent world point representing right side of screen
		viewportRight = camera.ViewportToWorldPoint (new Vector3 (0.99f, 0, transform.position.z)).x;
		// Current world point representing leftmost wander horizontal seek target
		xTargetLeft = camera.ViewportToWorldPoint (new Vector3 (0.05f, 0, transform.position.z)).x;
		// Current world point representing rightmost wander horizontal seek target
		xTargetRight = camera.ViewportToWorldPoint (new Vector3 (0.95f, 0, transform.position.z)).x;
		// Max vertical accel
		yAccMax = 5f;
		// Min vertical accel
		yAccMin = 1f;
		// Max vertical velocity
		yVelMax = 0.1f;
		// Lower offset from bottom of current screen position (used in narrow phase seek)
		ySeekOffset = -100f;
		// Screen height divisor
		ySeekSlice = 1.5f;
		ySeekSliceBot = 10f;

		// Init yTarget to yTargetBot, causes a swoop in from top of screen
		yTarget = yTargetTop;

		xTarget = transform.position.x;

		// Max horizontal accel
		xAccMax = 9f;
		// Min horiz accel
		xAccMin = 0.5f;
		// Max horiz velocity
		xVelMax = 0.01f;
		// Screen width divisor (for approximate horizontal bounds to init narrow phase seek)
		xSeekSlice = 6;

		height = viewportTop - viewportBot;

		xTargetMet = false;
		yTargetMet = false;

		// Determine which way I should face given my position and player's position
		// Set horizontal target in my direction
		if (player.transform.position.x - transform.position.x >= 0) {
			dir = 1;
		} else {
			dir = -1;
		}
		// Set enteredScreen bool to determine when to lock me to an upper screen boundary
		if (transform.position.y < viewportTop) {
			enteredScreen = true;
		} else {
			enteredScreen = false;
		}
	}

	// Update is called once per frame
	void Update () {
        // Swaps sprite in the correct direction
        if (xTarget == xTargetLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

	void FixedUpdate () {

		// BROAD PHASE SEEK
		if (seeking) {
			// X target is now player
			xTarget = player.transform.position.x;
		}
		if (seeking && !seekingPit) {
			// If in BPS but not NPS, reach the top of the screen (always be above player before attacking)
			yTarget = yTargetTop;
			// Set new X bounds to test if I am close enough to attack
			xSeekMin = player.transform.position.x - (Screen.width / xSeekSlice);
			xSeekMax = player.transform.position.x + (Screen.width / xSeekSlice);
			// NARROW PHASE SEEK
			// If I am reasonably close horizontally to player and I am near the top of the screen...
			if (xSeekMin < transform.position.x && transform.position.x < xSeekMax && transform.position.y >= yTarget) {
				seekingPit = true;
				// Final destination is off the bottom of the screen
				yTarget = viewportBot + ySeekOffset;
			}
			// --- NARROW PHASE SEEK END
		}
		// --- BPS END

		// Y VALUE MANIPULATION ------------------------------
		// ---------------------------------------------------

		// Set new Y targets and screen Y max/min world points

		viewportTop = camera.ViewportToWorldPoint(new Vector3(0, 1f, transform.position.z)).y;
		viewportBot = camera.ViewportToWorldPoint(new Vector3(0, 0f, transform.position.z)).y;

//		// Set the seek force to target
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

		// Make sure I actually entered from top of screen
		if (!enteredScreen && transform.position.y < yTarget) {
			enteredScreen = true;
		}
		// If entered from top already (visible) then limit me from going back up off the screen
		if (enteredScreen && transform.position.y > viewportTop) {
			transform.position = new Vector3 (transform.position.x, viewportTop);
		}



		// X VALUE MANIPULATION --------------------------------
		// -----------------------------------------------------

//		// Set X seek force
		xTargetForce = (xTarget - transform.position.x) * 10;
		// Limit X acc
		if (Mathf.Abs(xTargetForce) > xAccMax) {
			xTargetForce = xAccMax * Mathf.Sign(xTargetForce);
		}
		if (Mathf.Abs(xTargetForce) < xAccMin) {
			xTargetForce = xAccMin * Mathf.Sign(xTargetForce);
		}
		body.AddForce (new Vector2 (xTargetForce, 0));
		// Limit X velocity
		if (Mathf.Abs(body.velocity.x) > xVelMax) {
			float xval;
			// Add an opposite force represented by difference between yVel and yVelMax
			xval = -(body.velocity.x - (xVelMax * Mathf.Sign(body.velocity.x)));

			body.AddForce (new Vector2(xval, 0));
		}


		// Limit me from moving right/left off screen
		if (transform.position.x > viewportRight) {
			transform.position = new Vector3(viewportRight, transform.position.y);
		}
		if (transform.position.x < viewportLeft) {
			transform.position = new Vector3(viewportLeft, transform.position.y);
		}

		if (enteredScreen && !xSoughtOnce && !ySoughtOnce) {
			xTarget = player.transform.position.x;
			yTarget = player.transform.position.y;
			yTargetBot = player.transform.position.y - (height / ySeekSliceBot);
			yTargetTop = player.transform.position.y + (height / ySeekSlice);
		}

		if (!seeking) {
			if (!xSoughtOnce) {
				if (dir == 1 && transform.position.x > player.transform.position.x) {
					xSoughtOnce = true;
					xTarget = xTargetRight;
				} else if (dir == -1 && transform.position.x < player.transform.position.x) {
					xSoughtOnce = true;
					xTarget = xTargetLeft;
				}
			}
			if (!ySoughtOnce) {
				if (transform.position.y < yTarget) {
					yTarget = yTargetBot;
					ySoughtOnce = true;
				}
			}
			
			// both sought first
			if (xSoughtOnce && ySoughtOnce) {
				// reset lower target each frame if needed
				if (yTarget == yTargetBot && yTargetBot < (player.transform.position.y - (height / ySeekSlice))) {
					yTarget = yTargetBot = player.transform.position.y - (height / ySeekSliceBot);
				}

				// if seeking top and reached, but still lower than player, RESET top target before cycling phase
				if (yTarget == yTargetTop && (transform.position.y >= yTargetTop && yTargetTop <= player.transform.position.y || player.transform.position.y >= yTargetTop)) {
					yTarget = yTargetTop = player.transform.position.y + (height / ySeekSlice);
				}

				// if seeking bottom and reached, switch to top
				if (transform.position.y <= yTargetBot && yTarget == yTargetBot) {
					yTargetTop = player.transform.position.y + (height / ySeekSlice);
					yTarget = yTargetTop;
				}
				// if seeking top and reached, switch to bottom
				if (transform.position.y >= yTarget && yTarget == yTargetTop) {
					yTarget = yTargetBot;
					yTargetMet = true;
				}

				// reached top target, set new x target
				if (yTargetMet) {
					if (transform.position.x <= xTargetLeft && xTarget == xTargetLeft) {
						xTarget = xTargetRight;
						yTargetMet = false;
						ChangeDir (1);
					}
					if (transform.position.x >= xTargetRight && xTarget == xTargetRight) {
						xTarget = xTargetLeft;
						yTargetMet = false;
						ChangeDir (1);
					}
				}
			}

			// if reached x target
			if (transform.position.x <= xTargetLeft && xTarget == xTargetLeft) {
				xTargetMet = true;
			}
			if (transform.position.x >= xTargetRight && xTarget == xTargetRight) {
				xTargetMet = true;
			}


			// force to seek top if hit bottom of screen
			if (!seeking && transform.position.y < viewportBot) {
				yTarget = yTargetTop;
				transform.position = new Vector3(transform.position.x, viewportBot);
			}
		}
	}

	// Change the direction I am facing
	void ChangeDir (int newDir) {
		dir = newDir;
		timesFlipped++;
	}

	// MonoeyeSeekCollider calls me
	// Sets broad phase seek
	void OnEnemySeek () {
		if (timesFlipped >= timesFlippedMax) {
			seeking = true;
		}
	}
}
