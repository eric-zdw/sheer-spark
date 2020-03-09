using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffectRotate : MonoBehaviour {

	private float rx;
	private float ry;
	private float rz;

	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
		rx =  Random.Range(-1000f, 1000f);
		ry =  Random.Range(-1000f, 1000f);
		rz =  Random.Range(-1000f, 1000f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.rotation *= Quaternion.Euler(rx * Time.deltaTime, ry * Time.deltaTime, rz * Time.deltaTime);
	}
}
