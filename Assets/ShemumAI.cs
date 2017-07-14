using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShemumAI : MonoBehaviour {

    public float speed = 0.05f;
    public float waitTime = 1;

    private bool isLeft = false;
    private bool isWaiting = false;
    private float currentTime;
    private float startTime;
    private int layerIndex;
    private RaycastHit2D hit;
    private Collider2D collider;
    private Vector3 lastPosition;
    private GameObject player;
    private GameObject[] blocks;

	// Use this for initialization
	void Start () {
        collider = gameObject.GetComponent<Collider2D>();
        blocks = GameObject.FindGameObjectsWithTag("Block");
        layerIndex = 1 << LayerMask.NameToLayer("Block");
        player = GameObject.Find("Player");
    }

    // Method for reversing the direction of the Shemum
    void ReverseDirection()
    {
        speed *= -1;
        Wait();
    }

    // Method for making the Shemum wait
    void Wait()
    {
        isWaiting = true;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;

        // Shemum is actively moving
        if(!isWaiting)
        {
            transform.position += new Vector3(speed, 0, 0);
        }
        else // Shemum is paused at a wall or just fell
        {
            if(currentTime > (startTime + waitTime))
            {
                isWaiting = false;
            }
        }

        // Shoots the raycast in the proper direction
        if (isLeft)
        {
            hit = Physics2D.Raycast(transform.position, Vector3.left, 0.64f, layerIndex);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector3.right, 0.64f, layerIndex);
        }

        // Hits a block and reverses direction
        if (hit.collider != null && hit.collider.tag == "Block")
        {
            ReverseDirection();
            if(isLeft)
            {
                isLeft = false;
            }
            else
            {
                isLeft = true;
            }
        }
    }
}
