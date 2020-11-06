using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCannon : MonoBehaviour {

	public float fireRate;
	public GameObject projectile;
	private float fireTimer;

	public GameObject charge;
	private GameObject newCharge;
	public GameObject release;

	// Use this for initialization
	void Start () {
		fireTimer = fireRate;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		fireTimer -= Time.deltaTime;
		if (fireTimer <= 0f) {
			StartCoroutine(ShootRoutine());
			fireTimer = fireRate;
		}

		if (newCharge != null) {
			newCharge.transform.position = transform.position;
		}
	}

	IEnumerator ShootRoutine() {
		newCharge = Instantiate(charge, transform.position, transform.rotation * Quaternion.Euler(0f, -90f, 0f));
		yield return new WaitForSeconds(2f);
		GameObject newRelease = Instantiate(release, transform.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));
		Vector3 newRotation = transform.rotation.eulerAngles + new Vector3 (0f, 0f, 270f);
		Instantiate (projectile, transform.position + -transform.up * 0.2f, Quaternion.Euler(newRotation));
	}
}
