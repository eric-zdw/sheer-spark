using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFluctuation : MonoBehaviour {

	private float timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator WaveTimer() {
		while (true) {
			timer = Time.time % 20f;
		}
	}
}
