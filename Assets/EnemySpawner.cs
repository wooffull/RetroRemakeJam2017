using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemy;
	public float delay;
	public int enemyCount;
	private float lastTimeSpawned;
	private int enemiesSpawned;
	// Use this for initialization
	void Start () {
		enemiesSpawned = 0;
		lastTimeSpawned = Time.time;
		if (delay == null) {
			delay = 0.5f;
		}
		if (enemyCount == null) {
			enemyCount = 4;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (enemiesSpawned > 0 && (Time.time - lastTimeSpawned) > delay) {
			if (enemiesSpawned < enemyCount) {
				Instantiate (enemy, transform);
				enemiesSpawned++;
				lastTimeSpawned = Time.time;
			} else {
//				Destroy (gameObject);
				Destroy (this);
			}
		} else if (enemiesSpawned == 0) {
			Debug.Log ("first spawn");
			Instantiate (enemy, transform);
			enemiesSpawned++;
			lastTimeSpawned = Time.time;
		}
	}
}
