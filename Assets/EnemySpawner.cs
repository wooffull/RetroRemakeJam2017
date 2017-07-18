using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemy;
	public float delay;
	public int enemyCount;
	public float screenYOffsetUntilSpawn;
	private float lastTimeSpawned;
	private int enemiesSpawned;
	private new Camera camera;
	private bool inRange;
	// Use this for initialization
	void Start () {
		camera = Camera.main;
		if (screenYOffsetUntilSpawn == null) {
			screenYOffsetUntilSpawn = 5f;
		}
		enemiesSpawned = 0;
		lastTimeSpawned = Time.time;
		if (delay == null) {
			delay = 0.5f;
		}
		if (enemyCount == null) {
			enemyCount = 4;
		}
		inRange = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (inRange) {
			Debug.Log ("In range");
			if (enemiesSpawned > 0 && (Time.time - lastTimeSpawned) > delay) {
				if (enemiesSpawned < enemyCount) {
					Instantiate (enemy, transform);
					enemiesSpawned++;
					lastTimeSpawned = Time.time;
				} else {
					Destroy (this);
				}
			} else if (enemiesSpawned == 0) {
				Instantiate (enemy, transform);
				enemiesSpawned++;
				lastTimeSpawned = Time.time;
			}
		} else {
			if (transform.position.y - camera.ViewportToWorldPoint (new Vector3 (0, 1f, transform.position.z)).y < screenYOffsetUntilSpawn) {
				inRange = true;
			}
		}
	}
}
