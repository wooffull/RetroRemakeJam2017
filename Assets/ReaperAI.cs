using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAI : MonoBehaviour {

    public float speed = 0.05f;
    public float chargeSpeed = 0.1f;
    public float waitTime = 1;
    public float detectRange = 10;
    public float detectTime = 3;

    private bool isLeft = false;
    private bool isWaiting = false;
    private bool isHit = false;
    private bool playerDetected = false;
    private float currentTime;
    private float startTime;
    private float startDetectTime;
    private float halfWidth;
    private int layerIndex;
    private int playerLayerIndex;
    private int currentEnemyHealth;
    private RaycastHit2D hit;
    private RaycastHit2D playerHit;
    private Collider2D collider;
    private Vector3 lastPosition;
    private GameObject player;
    private GameObject[] blocks;
    private Stats enemyStats;
    private Vector3 leftSide;
    private Vector3 rightSide;
    private Vector3 currentSide;

    // Use this for initialization
    void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();
        blocks = GameObject.FindGameObjectsWithTag("Block");
        layerIndex = 1 << LayerMask.NameToLayer("Block");
        playerLayerIndex = 1 << LayerMask.NameToLayer("Player");
        player = GameObject.Find("Player");
        enemyStats = gameObject.GetComponent<Stats>();
        currentEnemyHealth = enemyStats.health;
        halfWidth = gameObject.GetComponent<BoxCollider2D>().size.x / 2;
        leftSide = gameObject.transform.position - new Vector3(halfWidth, 0, 0);
        rightSide = gameObject.transform.position + new Vector3(halfWidth, 0, 0);
        currentSide = leftSide;
    }

    // Method for reversing the direction of the Reaper
    void ReverseDirection()
    {
        speed *= -1;
        chargeSpeed *= -1;
        Wait();

        if (isLeft)
        {
            isLeft = false;
        }
        else
        {
            isLeft = true;
        }
    }

    // Method for making the Reaper wait
    void Wait()
    {
        isWaiting = true;
        startTime = Time.time;
    }

    // Method for updating points for raycasting
    void UpdatePoints()
    {
        leftSide = gameObject.transform.position - new Vector3(halfWidth, 0, 0);
        rightSide = gameObject.transform.position + new Vector3(halfWidth, 0, 0);
        if (isLeft)
        {
            currentSide = leftSide;
        }
        else
        {
            currentSide = rightSide;
        }
    }

    // Method for targeting the player
    void TargetPlayer()
    {
        Vector3 playerPosition = player.transform.position;

        if (playerPosition.x < transform.position.x && !isLeft)
        {
            ReverseDirection();
        }
        else if (playerPosition.x > transform.position.x && isLeft)
        {
            ReverseDirection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
        UpdatePoints();

        // Reaper pauses when hit
        if (enemyStats.health < currentEnemyHealth && !playerDetected)
        {
            Wait();
            currentEnemyHealth = enemyStats.health;
            isHit = true;
        }

        // Reaper is actively moving
        if (!isWaiting)
        {
            hit = Physics2D.Raycast(currentSide, Vector3.down, 1, layerIndex);

            // Hits an edge of a block
            if (hit.collider == null)
            {
                ReverseDirection();
            }
            else
            {
                if(playerDetected)
                {
                    transform.position += new Vector3(chargeSpeed, 0, 0);
                    TargetPlayer();
                }
                else
                {
                    transform.position += new Vector3(speed, 0, 0);
                }
            }
        }
        else // Reaper is paused at a wall or turned around randomly
        {
            if (currentTime > (startTime + waitTime))
            {
                isWaiting = false;

                // Reaper was hit and turns in the direction of the player
                if(isHit)
                {
                    TargetPlayer();
                    isHit = false;
                }
            }
        }

        // Shoots the raycast in the proper direction
        if (isLeft)
        {
            hit = Physics2D.Raycast(transform.position, Vector3.left, 0.64f, layerIndex);
            playerHit = Physics2D.Raycast(transform.position, Vector3.left, detectRange, playerLayerIndex);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector3.right, 0.64f, layerIndex);
            playerHit = Physics2D.Raycast(transform.position, Vector3.right, detectRange, playerLayerIndex);
        }

        // Hits a block and reverses direction
        if (hit.collider != null && hit.collider.tag == "Block")
        {
            ReverseDirection();
        }

        // Detects the player if they are found in sight
        if(playerHit.collider != null && playerHit.collider.tag == "Player")
        {
            startDetectTime = Time.time;
            playerDetected = true;
        }

        // Reaper stops chasing the player
        if(currentTime > (startDetectTime + detectTime))
        {
            playerDetected = false;
        }
    }
}
