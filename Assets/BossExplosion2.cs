using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosion2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 6f);
		StartCoroutine(LongShake());
	}
	
	private IEnumerator LongShake() {
		float timer = 3f;
		while (timer > 0f) {
			timer -= Time.deltaTime;
			Camera.main.GetComponent<CameraFollow>().AddNoise(50f * Time.deltaTime * timer);
			yield return new WaitForEndOfFrame();
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
