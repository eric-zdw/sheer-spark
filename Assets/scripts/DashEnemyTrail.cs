using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyTrail : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(DelayedStart());
		
	}

	public IEnumerator DelayedStart() {
		yield return new WaitForSeconds(0.1f);
		Material mat = transform.parent.GetComponent<MeshRenderer>().material;
		GetComponent<TrailRenderer>().material = mat;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
