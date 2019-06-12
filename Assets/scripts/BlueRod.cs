using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueRod : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3 (800f * Time.deltaTime, 0f, 0f));
	}
}
