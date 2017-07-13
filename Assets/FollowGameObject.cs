using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour {

    public GameObject target;

    private Transform targetTransform;
    private float minY;

	// Use this for initialization
	void Start () {
        targetTransform = target.GetComponent<Transform>();
        minY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (targetTransform.position.y > minY)
        {
            minY = targetTransform.position.y;
            transform.position = new Vector3(
                transform.position.x,
                minY,
                transform.position.z
            );
        }
	}
}
