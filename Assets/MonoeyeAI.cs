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
	private Vector3 viewportLeft;
	private Vector3 viewportRight;
	private Vector3 viewportLeftInner;
	private Vector3 viewportRightInner;
	private Vector3 viewportMax;
	private Vector3 viewportMin;
	private GameObject player;
	private int logiter;
	private float timeSinceStartSlowdown;
	private int timesFlipped;
	private float playerWidth;
	private RectTransform playerRect;
	private float xMax;
	private float xMin;
	// Use this for initialization
	void Start () {
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
		playerWidth = playerRect.rect.width;
		speed = 0.05f;
		player = GameObject.Find ("Player");
		if (player.transform.position.x - transform.position.x >= 0) {
			dir = 1;
		} else {
			dir = -1;
		}
		timeSinceStartSlowdown = Time.time - 0.01f;
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
		transform.SetPositionAndRotation (new Vector3(transform.position.x, yBand, transform.position.z), transform.rotation);
		transform.position += new Vector3 (speed * dir, 0);
		transform.position += 
			new Vector3(0, 0.8f * Mathf.Sin(2 * Mathf.PI * 0.5f * Time.time));
//		Debug.Log (Mathf.Sin (2 * Mathf.PI * 0.5f * Time.time));

		if (transform.position.x >= viewportRight.x) {
			ChangeDir (-1);
			timeSinceStartSlowdown = Time.time - 0.1f;
		} else if (transform.position.x <= viewportLeft.x) {
			ChangeDir (1);
			timeSinceStartSlowdown = Time.time - 0.1f;
		}

		if (transform.position.x >= viewportRightInner.x) {
			DeltaLog (1);
		} else if (transform.position.x <= viewportLeftInner.x) {
			DeltaLog (-1);
		}
		else {
			timeSinceStartSlowdown = Time.time - 0.1f;
			speed = 0.05f;
		}
		Debug.Log ("speed " +  speed);

		if (speed > 0.05f) {
			speed = 0.05f;
		}

		if (timesFlipped > 3) {
			if (transform.position.x < player.transform.position.x + playerWidth || transform.position.x > player.transform.position.x - playerWidth) {
				
			}
		}


	}

	void DeltaLog (int sign) {
//		Debug.Log ("Deltatime " + (Time.time - timeSinceStartSlowdown));
//		Debug.Log ("timesincestart " + timeSinceStartSlowdown);
		Debug.Log ("dir" + dir);

		if ((dir * sign) == 1) {
			speed -= (1 + Mathf.Log (Time.time - timeSinceStartSlowdown)) * 0.00035f;
		} else {
			speed += (1 + Mathf.Log (Time.time - timeSinceStartSlowdown)) * 0.00035f;
		}

		if (speed < 0.005f) {
			speed = 0.005f;
		}
	}

	void ChangeDir (int newDir) {
		dir = newDir;
		timesFlipped++;
	}
}
