using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupIndicator : MonoBehaviour {

    private Camera cam;
    Vector3 mousePos;

	// Use this for initialization
	void Start () {
        //cam = GameObject.FindGameObjectWithTag("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        //mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

    }
}
