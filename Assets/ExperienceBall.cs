using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceBall : Projectile {

	public GameObject playerTarget;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(Random.insideUnitSphere * (750f + (Random.value * 500f)));
	}
	
	// Update is called once per frame
	void Update () {
		rb.AddForce(Vector3.Normalize((playerTarget.transform.position) - (transform.position)) * Time.deltaTime * 1000f);
	}
}
