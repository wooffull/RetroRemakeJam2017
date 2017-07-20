using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    [HideInInspector]
    public float speed = 1.0f;

    [HideInInspector]
    public float maxLifeTime = 1.0f;

    [HideInInspector]
    public Vector3 direction = Vector3.right;

    [HideInInspector]
    public int damage = 1;
    
    private float lifeTime = 0.0f;
    private AudioManager audioManager;
    private List<GameObject> hitObjects;

    // Use this for initialization
    void Start ()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        hitObjects = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
        if (damage <= 0)
        {
            return;
        }

        // If this arrow has hit objects, find the nearest and handle it accordingly
        if (hitObjects.Count > 0)
        {
            GameObject nearest = hitObjects[0];
            float distToNearest = Vector2.SqrMagnitude(nearest.transform.position - transform.position);

            for (int i = 1; i < hitObjects.Count; i++)
            {
                GameObject cur = hitObjects[i];
                float distToCur = Vector2.SqrMagnitude(cur.transform.position - transform.position);
                
                if (distToCur < distToNearest)
                {
                    distToNearest = distToCur;
                    nearest = cur;
                }
            }

            // Damage the enemy and play a sound
            if (nearest.tag == "Enemy")
            {
                if (damage > 0)
                {
                    Stats stats = nearest.GetComponent<Stats>();
                    stats.health -= damage;
                    damage = 0;

                    audioManager.PlayTakeDamageSound();
                }
            }

            Destroy(gameObject);
            return;
        }

        Vector3 velocity = direction * speed * Time.deltaTime;
        transform.position += velocity;

        lifeTime += Time.deltaTime;

        if (lifeTime >= maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "Block")
        {
            hitObjects.Add(c.gameObject);
        }
    }
}
