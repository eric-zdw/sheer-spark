using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other) {
		other.transform.SetParent(gameObject.transform.parent);
	}

	void OnTriggerExit(Collider other) {
		other.transform.SetParent(null);
	}
}
