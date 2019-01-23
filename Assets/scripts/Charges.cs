using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charges : MonoBehaviour {

    RectTransform bar;
    DashBoost db;

	// Use this for initialization
	void Start () {
        bar = GetComponent<RectTransform>();
        db = GameObject.FindGameObjectWithTag("PlayerUtility").GetComponent<DashBoost>();
	}
	
	// Update is called once per frame
	void Update () {
        bar.localScale = new Vector3(1f * (db.charges / 3f), 1f, 1f);
	}
}
