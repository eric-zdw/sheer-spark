using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeProjectileExplosion : MonoBehaviour {
    
	private Light explosionLight;

	// Use this for initialization
	void Start () {
		explosionLight = GetComponent<Light>();
		Destroy(gameObject, 3f);
	} 
	
	// Update is called once per frame
	void Update () {
		explosionLight.intensity -= 3f * Time.deltaTime;
	}
}
