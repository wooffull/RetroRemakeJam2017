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
    private Sprite currentSprite;

	// Use this for initialization
	void Start () {
        camera = Camera.main;
        viewportBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        viewportTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));
        screenWidth = viewportTopRight.x - viewportBottomLeft.x;
        ghostPlayers = new GameObject[2];
        ghostPosition = transform.position;

        CreateGhostPlayers();
        PositionGhostPlayers();
    }

    void OnDestroy()
    {
        for (int i = 0; i < 2; i++)
        {
            Destroy(ghostPlayers[i]);
        }
    }

    // Method for creating ghost player sprites
    void CreateGhostPlayers()
    {
        for(int i = 0; i < 2; i++)
        {
            ghostPlayers[i] = new GameObject();
            sr = ghostPlayers[i].AddComponent<SpriteRenderer>();
            sr.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            sr.transform.rotation = gameObject.transform.rotation;
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

            ghostPlayers[i].transform.localScale = gameObject.transform.localScale;
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
