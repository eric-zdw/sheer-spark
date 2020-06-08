using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Vector2 start;
	public Vector2 end;

	public float cyclesPerSecond;

	private float time = 0f;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    time += Time.deltaTime;
		Vector2 newV = start + (Mathf.Cos(time * (Mathf.PI * 0.2f) * cyclesPerSecond) + 1f) * 0.5f * (end - start);
		rb.MovePosition(newV);
	}
}
