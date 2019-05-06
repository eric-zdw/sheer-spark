using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageShowcase : MonoBehaviour {

	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.rotation = transform.rotation * Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
	}
}
