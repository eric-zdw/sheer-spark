using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPad : MonoBehaviour {

    public float floatMagnitude;
    private float currentBoostCooldown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (currentBoostCooldown > 0f)
        {
            currentBoostCooldown -= Time.deltaTime;
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy") {
			other.GetComponent<Rigidbody>().AddForce(new Vector3(0f, floatMagnitude * Time.deltaTime, 0f));
		}
    }
}
