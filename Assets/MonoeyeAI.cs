using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoeyeAI : MonoBehaviour {

	private float intermittent_pos;
	private Collider2D collid;
	private new Camera camera;
	private float yBand;
	public float speed;
	private int dir;
	private Rigidbody2D body;
	private Vector3 viewportLeft;
	private Vector3 viewportRight;
	private Vector3 viewportLeftInner;
	private Vector3 viewportRightInner;
	private Vector3 viewportMax;
	private Vector3 viewportMin;
	private GameObject player;
	private int timesFlipped;
	private float playerWidth;
	private RectTransform playerRect;
	private float xMax;
	private float xMin;
	private float yVelMax;
	private float yBandForce;
	private float yAccMax;
	private float yAccMin;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		body = GetComponent<Rigidbody2D> ();
		collid = gameObject.GetComponent<Collider2D> ();
		timesFlipped = 0;
		camera = Camera.main;
		yBand = camera.ViewportToWorldPoint(new Vector3(0, 0.8f, transform.position.z)).y;
		viewportMin = camera.ViewportToWorldPoint (new Vector3 (0, 0, transform.position.z));
		viewportMax = camera.ViewportToWorldPoint (new Vector3 (1f, 0, transform.position.z));
		viewportLeft = camera.ViewportToWorldPoint (new Vector3 (0.1f, 0, transform.position.z));
		viewportRight = camera.ViewportToWorldPoint (new Vector3 (0.9f, 0, transform.position.z));
		viewportLeftInner = camera.ViewportToWorldPoint (new Vector3 (0.3f, 0, transform.position.z));
		viewportRightInner = camera.ViewportToWorldPoint (new Vector3 (0.7f, 0, transform.position.z));
		xMax = viewportRight.x;
		xMin = viewportLeft.x;
		yAccMax = 10f;
		yAccMin = 0.5f;
		yVelMax = 1.0f;
		//get the fucking collider 2d player and then get the width 
		speed = 0.05f;
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
		yBand = camera.ViewportToWorldPoint (new Vector3 (0, 0.8f, transform.position.z)).y;
		Debug.Log (body.velocity);
		yBandForce = (yBand - transform.position.y) * 15;
		if (Mathf.Abs(yBandForce) > yAccMax) {
			yBandForce = yAccMax * Mathf.Sign(yBandForce);
		}
		if (Mathf.Abs(yBandForce) < yAccMin) {
			yBandForce = yAccMin * Mathf.Sign(yBandForce);
		}
		body.AddForce (new Vector2 (0, yBandForce));
		Debug.Log ("new vel" + body.velocity);
		if (Mathf.Abs(body.velocity.y) > yVelMax) {
			float yval;
			if (body.velocity.y < 0) {
				yval = -(body.velocity.y + yVelMax);
			} else {
				yval = -(body.velocity.y - yVelMax);
			}
			Vector2 whocares = new Vector2(0, yval);
			body.AddForce (whocares);
		}
		transform.position += new Vector3 (speed * dir, 0);
		if (transform.position.x >= viewportRight.x) {
			ChangeDir (-1);
		} else if (transform.position.x <= viewportLeft.x) {
			ChangeDir (1);
		}

		if (speed > 0.05f) {
			speed = 0.05f;
		}
	}

	void ChangeDir (int newDir) {
		dir = newDir;
		timesFlipped++;
	}
}
