using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

	public GameObject mainMenuScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			MenuCameraRig.ChangePosition(1);
			mainMenuScreen.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
