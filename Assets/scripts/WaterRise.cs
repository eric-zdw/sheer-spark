using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRise : MonoBehaviour {

	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		StartCoroutine(WaterTide());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator WaterTide() {
		float time = 0f;
		while (true) {
			transform.position = startPosition + new Vector3(0f, Mathf.Sin((time * 0.025f) * Mathf.PI) * 15f, 0f);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
}
