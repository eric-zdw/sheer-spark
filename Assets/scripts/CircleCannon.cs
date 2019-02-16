using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCannon : MonoBehaviour {

	public float fireRate;
	public GameObject projectile;
	private float fireTimer;

	// Use this for initialization
	void Start () {
		fireTimer = fireRate;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		fireTimer -= Time.deltaTime;
		if (fireTimer <= 0f) {
			Vector3 newRotation = transform.rotation.eulerAngles + new Vector3 (0f, 0f, 270f);
			Instantiate (projectile, transform.position + -transform.up * 1f, Quaternion.Euler(newRotation));
			fireTimer += fireRate;
		}
	}
}
