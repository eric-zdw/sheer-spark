using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBossLasert : MonoBehaviour {

	private LineRenderer lr;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer>();
		transform.position = transform.parent.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 off = transform.parent.right * offset.x + transform.parent.up * offset.y + transform.parent.forward * offset.z;
		transform.position = transform.parent.position + off;
		lr.SetPosition(0, transform.position);

		Vector3 direction = transform.position - transform.parent.position;
		lr.SetPosition(1, transform.position + direction * 50f);
	}
}
