using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStick : MonoBehaviour {
	private Vector3 platformVelocity;
	private Rigidbody platformRB;

	private List<Rigidbody> movingList= new List<Rigidbody>();

	// Use this for initialization
	void Start () {
		platformRB = transform.parent.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		platformVelocity = platformRB.velocity;
		foreach (Rigidbody rb in movingList) {
			rb.AddForce(platformVelocity * (rb.mass * 150f) * Time.deltaTime);
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player")
			movingList.Add(other.GetComponent<Rigidbody>());
	}

	void OnTriggerExit(Collider other) {
		movingList.Remove(other.GetComponent<Rigidbody>());
	}
}
