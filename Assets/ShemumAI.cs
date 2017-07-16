using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShemumAI : MonoBehaviour {

    public float speed = 0.05f;
    public float waitTime = 1;
    public float pauseOnFallHeight = 3; // Measured in the scale of the block

    private bool isLeft = false;
    private bool isWaiting = false;
    private bool isFalling = false;
    private bool waitDisabled = false;
    private float currentTime;
    private float startTime;
    private float currentSpeed;
    private float halfWidth;
    private float blockScale;
    private int layerIndex;
    private RaycastHit2D hit;
    private Collider2D collider;
    private Vector3 lastPosition;
    private GameObject player;
    private GameObject[] blocks;
    private Vector3 leftSide;
    private Vector3 rightSide;
    private Vector3 currentSide;
    private Vector3 fallFromPosition;

    // Use this for initialization
    void Start () {
        collider = gameObject.GetComponent<Collider2D>();
        blocks = GameObject.FindGameObjectsWithTag("Block");
        layerIndex = 1 << LayerMask.NameToLayer("Block");
        player = GameObject.Find("Player");
        blockScale = gameObject.GetComponent<BoxCollider2D>().size.x;
        halfWidth = blockScale / 2;
        leftSide = gameObject.transform.position - new Vector3(halfWidth, 0, 0);
        rightSide = gameObject.transform.position + new Vector3(halfWidth, 0, 0);
        currentSide = leftSide;
        currentSpeed = speed;

        TargetPlayer();
    }

    // Method for reversing the direction of the Shemum
    void ReverseDirection()
    {
        currentSpeed *= -1;
        speed *= -1;

        if (isLeft)
        {
            isLeft = false;
        }
        else
        {
            isLeft = true;
        }
    }

    // Method for making the Shemum wait
    void Wait()
    {
        if(!waitDisabled)
        {
            isWaiting = true;
            startTime = Time.time;
        }
        else
        {
            waitDisabled = false;
        }
    }

    // Method for updating points for raycasting
    void UpdatePoints()
    {
        leftSide = gameObject.transform.position - new Vector3(halfWidth, 0, 0);
        rightSide = gameObject.transform.position + new Vector3(halfWidth, 0, 0);
        if (isLeft)
        {
            currentSide = rightSide;
        }
        else
        {
            currentSide = leftSide;
        }
    }

    // Method for targeting the player
    void TargetPlayer()
    {
        Vector3 playerPosition = player.transform.position;

        if(playerPosition.x < transform.position.x && !isLeft)
        {
            ReverseDirection();
        }
        else if(playerPosition.x > transform.position.x && isLeft)
        {
            ReverseDirection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
        UpdatePoints();
        // Shemum is actively moving
        if (!isWaiting)
        {
            hit = Physics2D.Raycast(currentSide, Vector3.down, 1, layerIndex);

            // Lands on a block
            if (isFalling && hit.collider != null && hit.collider.tag == "Block")
            {
                isFalling = false;
                currentSpeed = speed; // Resume movement

                // Pauses if the fall passes the fall height
                if(fallFromPosition.y - transform.position.y > blockScale * pauseOnFallHeight)
                {
                    Wait();
                }
                else
                {
                    waitDisabled = true;
                }
            }

            // Falls off the edge of a block
            if (hit.collider == null)
            {
                currentSpeed = 0; // Stop movement
                TargetPlayer();
                if(!isFalling)
                {
                    isFalling = true;
                    fallFromPosition = transform.position;
                }
            }
            else
            {
                transform.position += new Vector3(currentSpeed, 0, 0);
            }
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
            Wait();
        }
    }
}
