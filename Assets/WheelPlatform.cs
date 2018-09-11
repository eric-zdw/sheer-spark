using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPlatform : MonoBehaviour {

	public float rotateRate = 60f;
	public float radius = 20f;
	public Vector3 center;
	public float startAngle = 0f;

	private float currentAngle;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		currentAngle = startAngle;
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		currentAngle += Time.deltaTime * rotateRate;
		rb.MovePosition (center + new Vector3 (radius * Mathf.Cos (Mathf.Deg2Rad * currentAngle), radius * Mathf.Sin (Mathf.Deg2Rad * currentAngle), transform.position.z));
		//transform.position = center + new Vector3 (radius * Mathf.Cos (Mathf.Deg2Rad * currentAngle), radius * Mathf.Sin (Mathf.Deg2Rad * currentAngle), transform.position.z);
	}
}
