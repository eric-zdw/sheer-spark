using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPowerup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator ColourShift() {
		while (true) {
			yield return new WaitForEndOfFrame();
		}
	}
}
