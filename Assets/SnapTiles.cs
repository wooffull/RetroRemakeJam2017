using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapTiles : MonoBehaviour {
    public float snapValue = 1.28f;
    public float depth = 0;

    void Start()
    {
        if (!Application.isEditor || Application.isPlaying)
        {
            Destroy(this);
        }
    }
    
    void Update()
    {
        float snapInverse = 1 / snapValue;

        float x, y, z;

        // if snapValue = .5, x = 1.45 -> snapInverse = 2 -> x*2 => 2.90 -> round 2.90 => 3 -> 3/2 => 1.5
        // so 1.45 to nearest .5 is 1.5
        x = Mathf.Round(transform.position.x * snapInverse) / snapInverse;
        y = Mathf.Round(transform.position.y * snapInverse) / snapInverse;
        z = depth;  // depth from camera

        transform.position = new Vector3(x, y, z);
    }
}