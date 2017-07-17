using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecknoseAI : MonoBehaviour {

    public float speed = 0.05f;

    private bool isLeft = false;
    private bool startExponentialReverse = false;
    private int pathPattern;
    private int layerIndex;
    private float currentSpeed;
    private RaycastHit2D hit;
    private Collider2D collider;

    // Use this for initialization
    void Start () {
        layerIndex = 1 << LayerMask.NameToLayer("Wall");
        currentSpeed = speed;

        // Random pattern selection
        float num = Random.value;
        if(num < 0.5)
        {
            pathPattern = 0;
        }
        else
        {
            pathPattern = 1;
        }

        // Random starting direciton
        num = Random.value;
        if(num < 0.5)
        {
            currentSpeed *= -1;
            speed *= -1;
        }
	}

    // Method for reversing direction
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

    // Method for reversing direction smoothly
    void ExponentialReverseDirection()
    {
        if(startExponentialReverse)
        {
            currentSpeed += speed / 10;
            if(currentSpeed == 0)
            {
                speed *= -1;
                ReverseDirection();
            }

            // Catches back up to full speed in the other direction
            if(currentSpeed >= speed)
            {
                currentSpeed = speed;
                startExponentialReverse = false;
            }
        }
    }

    // Method for moving the Specknose back and forth
    void MoveHorizontally()
    {
        if(isLeft)
        {
            hit = Physics2D.Raycast(transform.position, Vector3.left, 3, layerIndex);

            if(hit.collider != null && hit.collider.tag == "Wall")
            {
                if (!startExponentialReverse)
                {
                    speed *= -1;
                }
                startExponentialReverse = true;
            }
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector3.right, 3, layerIndex);

            if (hit.collider != null && hit.collider.tag == "Wall")
            {
                if (!startExponentialReverse)
                {
                    speed *= -1;
                }
                startExponentialReverse = true;
            }
        }

        ExponentialReverseDirection();
        transform.position += new Vector3(currentSpeed, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		if(pathPattern == 0)
        {
            MoveHorizontally();
        }
        else if(pathPattern == 1)
        {
            MoveHorizontally();
        }
	}
}
