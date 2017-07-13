using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Screen Wrap Ghost Creation credit to Damir Veapi
 * https://gamedevelopment.tutsplus.com/articles/create-an-asteroids-like-screen-wrapping-effect-with-unity--gamedev-15055
 */
public class ScreenWrap : MonoBehaviour {

    private Rigidbody2D rigidBody;
    private Camera camera;
    private Vector3 viewportBottomLeft;
    private Vector3 viewportTopRight;
    private float screenWidth;
    private float screenHeight;
    private Transform[] ghostPlayers;
    private Vector3 ghostPosition;

	// Use this for initialization
	void Start () {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        camera = Camera.main;
        viewportBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        viewportTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));
        screenWidth = viewportTopRight.x - viewportBottomLeft.x;
        screenHeight = viewportTopRight.y - viewportBottomLeft.y;
        ghostPlayers = new Transform[2];
        ghostPosition = transform.position;

        //CreateGhostPlayers(); - Causes crashes currently
    }

    // Method for creating ghost player sprites
    void CreateGhostPlayers()
    {
        /// No idea what the hell any of this does
        for(int i = 0; i < 2; i++)
        {
            ghostPlayers[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;

            //DestroyImmediate(ghostPlayers[i].GetComponent());
        }
    }

    // Method for setting the ghost player sprite positions
    void PositionGhostPlayers()
    {
        // Right side
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghostPlayers[0].position = ghostPosition;

        // Left side
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghostPlayers[1].position = ghostPosition;
    }
	
	// Update is called once per frame
	void Update () {
        foreach(Transform ghost in ghostPlayers)
        {
            if (ghost.position.x < screenWidth && ghost.position.x > -screenWidth &&
            ghost.position.y < screenHeight && ghost.position.y > -screenHeight)
            {
                transform.position = ghost.position;

                break;
            }
        }

        PositionGhostPlayers();
	}
}
