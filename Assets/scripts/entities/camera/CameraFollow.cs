using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject followTarget;
    public int CameraDistance;
    Camera cam;

	private float shakeFactor;

	// Use this for initialization
	void Start ()
    {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 newPosition;
        //inital camera position
        newPosition = followTarget.transform.position + new Vector3(0, 0, -CameraDistance);

        //add mouse positioning
        newPosition += cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraDistance));
        newPosition = new Vector3(newPosition.x / 2, newPosition.y / 2, -CameraDistance);
        transform.position = newPosition;

		//shakeFactor /=
    }

	public void addShake(float magnitude)
	{

	}
}
