using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyWhenBelowCamera : MonoBehaviour
{
    // Instead of destroying things when they fall below the camera, allow an offset so
    // things can still be allowed a little below
    public float bottomOffset = 1.28f;

    private new Camera camera;
    private new Collider2D collider;
	private Vector3 viewportBottomLeft;
    private Vector3 viewportTopRight;

    // Use this for initialization
    void Start ()
    {
        camera = Camera.main;
        viewportBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        // viewportTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));
        collider = GetComponent<Collider2D>();
	    }
	
	// Update is called once per frame
	void Update () {
		
		viewportBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
		// viewportTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

		if (collider.bounds.max.y < viewportBottomLeft.y - bottomOffset)
        {
            Destroy(gameObject);

			if (gameObject.tag == "Player")
            {
                SceneManager.LoadScene("GameOverScene");
            }
        }
	}
}
