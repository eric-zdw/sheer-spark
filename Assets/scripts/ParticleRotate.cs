using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotate : MonoBehaviour {

    public bool rotateUp;
    public bool rotateRight;
    public bool rotateForward;
    public float magnitudeUp;
    public float magnitudeRight;
    public float magnitudeForward;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (rotateUp)
            transform.RotateAround(transform.parent.position, Vector3.up, magnitudeUp * Time.deltaTime);
        if (rotateRight)
            transform.RotateAround(transform.parent.position, Vector3.right, magnitudeRight * Time.deltaTime);
        if (rotateForward)
            transform.RotateAround(transform.parent.position, Vector3.forward, magnitudeForward * Time.deltaTime);
    }
}
