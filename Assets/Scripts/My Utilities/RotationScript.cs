using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {

    public bool rotate = false;
    public float speed = 1f;
    Transform myTransform;


    void Start()
    {
        myTransform = transform;
    }

	void FixedUpdate()
    {
        if (!rotate)
            return;

        myTransform.Rotate(new Vector3(0f, 0f, speed));
    }
}
