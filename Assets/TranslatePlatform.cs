using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslatePlatform : MonoBehaviour {

	public float moveRate = 0.1f;
	public float currentLerp = 0f;
	public Vector3 startPos;
	public Vector3 endPos;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		transform.position = Vector3.Lerp (startPos, endPos, currentLerp);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		currentLerp += Time.deltaTime * moveRate;
		if (currentLerp >= 1f)
			currentLerp -= 1f;
		rb.MovePosition(Vector3.Lerp(startPos, endPos, currentLerp));
		//transform.position = center + new Vector3 (radius * Mathf.Cos (Mathf.Deg2Rad * currentAngle), radius * Mathf.Sin (Mathf.Deg2Rad * currentAngle), transform.position.z);
	}
}
