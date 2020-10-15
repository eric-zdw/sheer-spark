using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionlight : MonoBehaviour {

    private Light expLight;

	// Use this for initialization
	void Start () {
        expLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        expLight.intensity -= 10 * Time.deltaTime;
	}
}