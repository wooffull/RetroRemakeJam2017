using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public Sprite arrowSprite;
    public float arrowSpeed = 0.25f;
    public GameObject arrowPrefab;

    private bool arrowActive;
    private bool shootLeft;
    private bool shootUp;
    private Vector3 startPosition;
    private GameObject arrow;
    private GameObject player;
    private Collider2D arrowCollider;
    private Collider2D playerCollider;
    private PlayerMovement playerMovement;
    private Stats playerStats;
    private GameObject[] enemies;
    private GameObject[] blocks;

	// Use this for initialization
	void Start () {
        arrow = GameObject.Find("Arrow");
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerCollider = gameObject.GetComponent<Collider2D>();
        playerStats = gameObject.GetComponent<Stats>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        blocks = GameObject.FindGameObjectsWithTag("Block");
    }

    // Method for spawning an arrow into the game
    void SpawnArrow()
    {
        arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        if(playerMovement.isUp)
        {
            // Spawns upward
            arrow.transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y + 2.24f,
                player.transform.position.z
                );
            arrow.transform.rotation = new Quaternion(45, 45, 0, 0);
            shootUp = true;
        }
        else if(playerMovement.isLeft)
        {
            // Spawns to the left
            arrow.transform.position = new Vector3(
                player.transform.position.x - 1.28f, 
                player.transform.position.y + .96f, 
                player.transform.position.z);
            arrow.transform.localScale = new Vector3(-1, 1, 1);
            shootLeft = true;
            shootUp = false;
        }
        else
        {
            // Spawns to the right
            arrow.transform.position = new Vector3(
                player.transform.position.x + 1.28f,
                player.transform.position.y + .96f,
                player.transform.position.z);
            shootLeft = false;
            shootUp = false;
        }

        arrowCollider = arrow.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(arrowCollider, playerCollider);
        arrowActive = true;
        startPosition = arrow.transform.position;
    }

    // Method for making the arrow move
    void MoveArrow()
    {
        if(shootUp)
        {
            arrow.transform.position += new Vector3(0, arrowSpeed, 0);
        }
        else if(shootLeft)
        {
            arrow.transform.position += new Vector3(-arrowSpeed, 0, 0);
        }
        else
        {
            arrow.transform.position += new Vector3(arrowSpeed, 0, 0);
        }

        if (arrow.transform.position.x < startPosition.x - 3.84 || arrow.transform.position.x > startPosition.x + 3.84 || arrow.transform.position.y > startPosition.y + 3.84) // Arrow has traveled 3 blocks left, right, or upward
        {
            DestroyArrow();
        }
    }

    // Method for destroying an arrow
    void DestroyArrow()
    {
        arrowActive = false;
        Destroy(arrow);
    }

    // Method for updating the arrays
    void UpdateArrays()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        blocks = GameObject.FindGameObjectsWithTag("Block");
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1") && !arrowActive && !playerMovement.isCrouch)
        {
            SpawnArrow();
        }

        if(arrowActive)
        {
            MoveArrow();

            // Destroys the arrow if it touches a block
            for (int i = 0; i < blocks.Length; i++)
            {
                if (arrowCollider.IsTouching(blocks[i].GetComponent<Collider2D>()))
                {
                    DestroyArrow();
                }
            }

            // Destroys the arrow if it touches an enemy
            for (int i = 0; i < enemies.Length; i++)
            {
                if (arrowCollider.IsTouching(enemies[i].GetComponent<Collider2D>()))
                {
                    enemies[i].GetComponent<Stats>().health -= playerStats.damage;
                    DestroyArrow();
                }
            }

        }

        UpdateArrays();
	}
}
