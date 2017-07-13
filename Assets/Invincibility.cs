using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour {

    public float invincibilityTime = 60; // Measured in frames
    public bool isInvincible = false;

    private int currentFrame;
    private int startFrame;

	// Use this for initialization
	void Start () {
	}

    // Method for making the object invincible
    public void MakeInvincible()
    {
        isInvincible = true;
        startFrame = Time.frameCount;
    }
	
	// Update is called once per frame
	void Update () {
        currentFrame = Time.frameCount;

        // If the invincibility time has passed, set invincibility off
		if(isInvincible && currentFrame > (startFrame + invincibilityTime))
        {
            isInvincible = false;
        }
	}
}
