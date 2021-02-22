using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : MonoBehaviour {

    private float rotation;

	// Use this for initialization
	void Start () {
        rotation = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        rotation += 5f * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
	}
}
