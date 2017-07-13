using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalInput : MonoBehaviour {

    public bool isCrouch = false;
    public bool isUp = false;

    private GameObject player = GameObject.Find("Player");
    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start()
    {
        rigidBody = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        // Pit crouches
        if (verticalInput < 0)
        {
            isCrouch = true;
            isUp = false;
        }
        else if (verticalInput > 0) // Pit aims upward
        {
            isCrouch = false;
            isUp = true;
        }
        else
        {
            isCrouch = false;
            isUp = false;
        }
    }
}
