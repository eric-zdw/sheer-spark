using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionlight : MonoBehaviour {

    private Light light;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        light.intensity -= 10 * Time.deltaTime;
	}
}