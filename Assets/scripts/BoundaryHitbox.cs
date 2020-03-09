using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryHitbox : MonoBehaviour {

	public Vector3 force;
	public Boundary boundary;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<Rigidbody>().velocity = force;
		}
		boundary.HitFlash();
	}
}
