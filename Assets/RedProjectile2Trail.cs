using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedProjectile2Trail : MonoBehaviour {

	private TrailRenderer trail;

	// Use this for initialization
	void Start () {
		trail = GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		trail.widthMultiplier *= 0.8f;
	}
}
