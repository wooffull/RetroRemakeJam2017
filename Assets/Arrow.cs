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

    private bool arrowDisabled = false;
    private float lifeTime = 0.0f;
    private AudioManager audioManager;

    // Use this for initialization
    void Start ()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
    }
	
	// Update is called once per frame
	void Update () {
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
        // Ensures the arrow doesn't affect enemies through bocks
        if(c.gameObject.tag == "Block")
        {
            arrowDisabled = true;
        }

        // Damage the enemy and play a sound
        if (c.gameObject.tag == "Enemy" && !arrowDisabled)
        {
            Stats stats = c.gameObject.GetComponent<Stats>();
            stats.health -= damage;

            audioManager.PlayTakeDamageSound();
        }

        if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "Block")
        {
            Destroy(gameObject);
        }
    }
}
