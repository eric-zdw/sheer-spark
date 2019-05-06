using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObject : MonoBehaviour {

	private Camera cam;
	public Vector3 mouseLocation;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		mouseLocation = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f));
		transform.position = mouseLocation;
	}
}
