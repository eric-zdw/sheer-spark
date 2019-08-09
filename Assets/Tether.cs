using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour {

	private float tetherTimer = 0.25f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		tetherTimer -= Time.deltaTime;
		if (tetherTimer < 0f) {
			Destroy(gameObject);
		}
	}

	public void ResetTimer() {
		tetherTimer = 0.25f;
	}
}
