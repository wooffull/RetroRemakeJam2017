using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAI : MonoBehaviour {

    public float speed = 0.05f;
    public float chargeSpeed = 0.1f;
    public float waitTime = 1;
    public float detectRange = 10;
    public float detectTime = 6;
    public float turnInterval = 3;
	public GameObject reapetteSpawner;

    private bool isLeft = false;
    private bool isWaiting = false;
    private bool isHit = false;
    private bool isTurning = false;
    private bool isTurningDisabled = false;
    private bool playerDetected = false;
    private bool playerTargeted = false;
    private float currentTime;
    private float startTime;
    private float startDetectTime;
    private float startTurnTime;
    private float halfWidth;
	private float screenTop;
	private float lastSpawnTime;
	private float reapetteSpawnerTimeBuffer;
    private int layerIndex;
    private int playerLayerIndex;
    private int currentEnemyHealth;
    private RaycastHit2D hit;
    private RaycastHit2D playerHit;
    private RaycastHit2D blockHit;
    private Collider2D collider;
    private Vector3 lastPosition;
    private GameObject player;
    private GameObject[] blocks;
    private Stats enemyStats;
    private Vector3 leftSide;
    private Vector3 rightSide;
    private Vector3 currentSide;
    private AudioManager audioManager;
	private new Camera camera;

    // Use this for initialization
    void Start()
    {
		reapetteSpawnerTimeBuffer = 5f;
		lastSpawnTime = 0;
		camera = Camera.main;
		screenTop = camera.ViewportToWorldPoint (new Vector3(0, 1f, transform.position.z)).y;
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
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
    }

    // Method for reversing the direction of the Reaper
    void ReverseDirection()
    {
        speed *= -1;
        chargeSpeed *= -1;

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
		screenTop = camera.ViewportToWorldPoint (new Vector3(0, 1f, transform.position.z)).y;
        currentTime = Time.time;
        UpdatePoints();

        // Reaper pauses when hit
        if (enemyStats.health < currentEnemyHealth && !playerDetected)
        {
            Wait();
            currentEnemyHealth = enemyStats.health;
            isHit = true;
        }

        // Reaper turns around at the set interval
        if(currentTime > (startTurnTime + turnInterval) && !isTurningDisabled)
        {
            startTurnTime = Time.time;
            isTurning = true;
            ReverseDirection();
            Wait();
        }

        // Reaper is actively moving
        if (!isWaiting)
        {
            hit = Physics2D.Raycast(currentSide, Vector3.down, 1, layerIndex);

            // Hits an edge of a block
            if (hit.collider == null)
            {
                ReverseDirection();
                Wait();
            }
            else
            {
                if(playerDetected)
                {
                    transform.position += new Vector3(chargeSpeed, 0, 0);
                    if(!playerTargeted)
                    {
                        TargetPlayer();
                        playerTargeted = true;
                    }
                }
                else
                {
                    transform.position += new Vector3(speed, 0, 0);
                    playerTargeted = false;
                }
            }
        }
        else // Reaper is paused at a wall or turned around randomly
        {
            if (currentTime > (startTime + waitTime))
            {
                isWaiting = false;

                // Reaper resumes the direction it turned from
                if(isTurning && !isTurningDisabled)
                {
                    ReverseDirection();
                    isTurning = false;
                    startTurnTime = Time.time;
                }

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
            blockHit = Physics2D.Raycast(transform.position, Vector3.left, detectRange, layerIndex);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector3.right, 0.64f, layerIndex);
            playerHit = Physics2D.Raycast(transform.position, Vector3.right, detectRange, playerLayerIndex);
            blockHit = Physics2D.Raycast(transform.position, Vector3.left, detectRange, layerIndex);
        }

        // Hits a block and reverses direction
        if (hit.collider != null && hit.collider.tag == "Block")
        {
            ReverseDirection();
            Wait();
        }

        // Detects the player if they are found in sight
        if(playerHit.collider != null && playerHit.collider.tag == "Player" && playerHit.distance < blockHit.distance)
        {
            startDetectTime = Time.time;

            if (playerDetected == false)
            {
                audioManager.PlayReaperSpotsPlayerSound();
            }

            playerDetected = true;
            isWaiting = false;
            isTurningDisabled = true;
            isTurning = false;

			if (GameObject.Find ("Reapette(Clone)") == null && Time.time - lastSpawnTime > reapetteSpawnerTimeBuffer) {
				Instantiate (reapetteSpawner, new Vector3(player.transform.position.x, screenTop + 2f), Quaternion.identity);
				lastSpawnTime = Time.time;
			}
        }

        // Reaper stops chasing the player
        if(currentTime > (startDetectTime + detectTime))
        {
            if(playerDetected)
            {
                playerDetected = false;
                isTurningDisabled = false;
                startTurnTime = Time.time;
            }
        }
    }
}
