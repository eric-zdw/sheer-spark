using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeProjectileExplosion : MonoBehaviour {
    
	private Light light;

	// Use this for initialization
	void Start () {
		light = GetComponent<Light>();
		Destroy(gameObject, 3f);
	} 
	
	// Update is called once per frame
	void Update () {
		light.intensity -= 3f * Time.deltaTime;
	}
}
