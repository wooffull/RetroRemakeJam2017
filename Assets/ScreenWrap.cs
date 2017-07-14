using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Screen Wrap Ghost Creation credit to Damir Veapi
 * https://gamedevelopment.tutsplus.com/articles/create-an-asteroids-like-screen-wrapping-effect-with-unity--gamedev-15055
 */
public class ScreenWrap : MonoBehaviour {
    
    private new Camera camera;
    private Vector3 viewportBottomLeft;
    private Vector3 viewportTopRight;
    private float screenWidth;
    private GameObject[] ghostPlayers;
    private Vector3 ghostPosition;
    private SpriteRenderer sr;
    private GameObject player;
    private Sprite currentSprite;
    private PlayerMovement playerMovement;

	// Use this for initialization
	void Start () {
        camera = Camera.main;
        viewportBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        viewportTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));
        screenWidth = viewportTopRight.x - viewportBottomLeft.x;
        ghostPlayers = new GameObject[2];
        ghostPosition = transform.position;
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        CreateGhostPlayers();
        PositionGhostPlayers();
    }

    // Method for creating ghost player sprites
    void CreateGhostPlayers()
    {
        /// No idea what the hell any of this does
        for(int i = 0; i < 2; i++)
        {
            ghostPlayers[i] = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity) as GameObject;
            sr = ghostPlayers[i].AddComponent<SpriteRenderer>();
            sr.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    // Method for updating the ghost player sprites
    void UpdateGhostPlayers()
    {
        for(int i = 0; i < 2; i++)
        {
            // Finds the current sprite of the player and sets it to the ghost sprites
            currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            sr = ghostPlayers[i].GetComponent<SpriteRenderer>();
            sr.sprite = currentSprite;

            if(playerMovement.isLeft)
            {
                ghostPlayers[i].transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                ghostPlayers[i].transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    // Method for setting the ghost player sprite positions
    void PositionGhostPlayers()
    {
        // Right side
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghostPlayers[0].transform.position = ghostPosition;

        // Left side
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghostPlayers[1].transform.position = ghostPosition;
    }
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject ghost in ghostPlayers)
        {
            if (ghost.transform.position.x < screenWidth * 0.5f && ghost.transform.position.x > -screenWidth * 0.5f)
            {
                transform.position = ghost.transform.position;

                break;
            }
        }

        PositionGhostPlayers();
        UpdateGhostPlayers();
	}
}
