using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystem>().time = 50;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
