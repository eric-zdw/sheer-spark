using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSphereLayer : MonoBehaviour {

	Vector3 startPosition;
	Quaternion rotation;
	float retractFactor;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;

		retractFactor = Mathf.Sin(Time.time * 0.1f * Mathf.PI) * 5f;

		transform.GetChild(0).position = startPosition + new Vector3(Mathf.Cos(0f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(0f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(1).position = startPosition + new Vector3(Mathf.Cos(60f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(60f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(2).position = startPosition + new Vector3(Mathf.Cos(120f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(120f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(3).position = startPosition + new Vector3(Mathf.Cos(180f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(180f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(4).position = startPosition + new Vector3(Mathf.Cos(240f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(240f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(5).position = startPosition + new Vector3(Mathf.Cos(300f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(300f * Mathf.Deg2Rad) * retractFactor);

		StartCoroutine(Rotate());
		StartCoroutine(Retract());
	}

	void FixedUpdate() {
		transform.GetChild(0).position = startPosition + new Vector3(Mathf.Cos(0f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(0f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(1).position = startPosition + new Vector3(Mathf.Cos(60f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(60f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(2).position = startPosition + new Vector3(Mathf.Cos(120f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(120f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(3).position = startPosition + new Vector3(Mathf.Cos(180f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(180f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(4).position = startPosition + new Vector3(Mathf.Cos(240f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(240f * Mathf.Deg2Rad) * retractFactor);
		transform.GetChild(5).position = startPosition + new Vector3(Mathf.Cos(300f * Mathf.Deg2Rad) * retractFactor, Mathf.Sin(300f * Mathf.Deg2Rad) * retractFactor);
	}
	
	IEnumerator Rotate() {
		while (true) {
			transform.rotation *= Quaternion.Euler(0f, 0f, Mathf.Sin((Time.time * 0.05f) * Mathf.PI) * 8f);
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator Retract() {
		while (true) {
			retractFactor = Mathf.Sin(Time.time * 1f * Mathf.PI) * 5f;
			yield return new WaitForEndOfFrame();
		}
	}
	
}
