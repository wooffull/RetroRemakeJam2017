using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemy;
	public float delay = 0.5f;
	public int enemyCount = 4;
	public float screenYOffsetUntilSpawn = 5f;
	private float lastTimeSpawned;
	private int enemiesSpawned;
	private new Camera camera;
	private bool inRange;
	// Use this for initialization
	void Start () {
		camera = Camera.main;
		enemiesSpawned = 0;
		lastTimeSpawned = Time.time;
		inRange = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (inRange) {
			if (enemiesSpawned > 0 && (Time.time - lastTimeSpawned) > delay) {
				if (enemiesSpawned < enemyCount) {
					Instantiate (enemy, transform.position, Quaternion.identity);
					enemiesSpawned++;
					lastTimeSpawned = Time.time;
				} else {
					Destroy (this);
				}
			}
            else if (enemiesSpawned == 0)
            {
                Instantiate(enemy, transform.position, Quaternion.identity);
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
