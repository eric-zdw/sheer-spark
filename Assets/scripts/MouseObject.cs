using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObject : MonoBehaviour {

	private Camera cam;
	private CameraFollow camScript;
	public Vector3 mouseLocation;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		camScript = cam.GetComponent<CameraFollow>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraFollow.CameraDistance));
	}
}
