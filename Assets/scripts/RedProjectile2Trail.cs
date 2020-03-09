using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedProjectile2Trail : MonoBehaviour {

	private TrailRenderer trail;

	// Use this for initialization
	void Start () {
		trail = GetComponent<TrailRenderer>();
		StartCoroutine(Fade());
		Destroy(gameObject, 2.5f);
	}

	IEnumerator Fade() {
		while (true) {
			Color newColor = new Color(trail.startColor.r, trail.startColor.g, trail.startColor.b, trail.startColor.a * 0.75f);
			trail.widthMultiplier *= 0.75f;
			trail.startColor = newColor;
			trail.endColor = newColor;
			yield return new WaitForSeconds(0.02f);
		}
	}
}
