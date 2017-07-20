using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public Sprite arrowSprite;
    public float arrowTravelDistance = 3 * 1.28f;
    public float arrowTravelTime = 1.0f;
    public GameObject arrowPrefab;

    [HideInInspector]
    public GameObject arrow;

    private GameObject player;
    private PlayerAnimations playerAnimations;
    private PlayerMovement playerMovement;
    private Stats playerStats;
    private AudioManager audioManager;

    void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAnimations = player.GetComponent<PlayerAnimations>();
        playerStats = gameObject.GetComponent<Stats>();
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (!playerAnimations.shootActive && Input.GetButtonDown("Fire1") && arrow == null && !playerMovement.isCrouch)
        {
            SpawnArrow();
        }
    }

    private void SpawnArrow()
    {
        arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        Arrow arrowComponent = arrow.GetComponent<Arrow>();

        arrowComponent.speed = arrowTravelDistance / arrowTravelTime;
        arrowComponent.maxLifeTime = arrowTravelTime;
        arrowComponent.damage = playerStats.damage;
        audioManager.PlayShootArrowSound();

        if (playerMovement.isUp)
        {
            // Spawns upward
            arrow.transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y + 2f,
                player.transform.position.z
                );
            arrow.transform.rotation = new Quaternion(45, 45, 0, 0);
            arrowComponent.direction = Vector3.up;
        }
        else if (playerMovement.isLeft)
        {
            // Spawns to the left
            arrow.transform.position = new Vector3(
                player.transform.position.x - 1f,
                player.transform.position.y + 1f,
                player.transform.position.z);
            arrow.transform.localScale = new Vector3(-1, 1, 1);
            arrowComponent.direction = Vector3.left;
        }
        else
        {
            // Spawns to the right
            arrow.transform.position = new Vector3(
                player.transform.position.x + 1f,
                player.transform.position.y + 1f,
                player.transform.position.z);
            arrowComponent.direction = Vector3.right;
        }
    }
}
