using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour {
    private float radius = 8f;
    public float explosionForce;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Enemy") {
            float distance = Vector3.Distance(other.ClosestPointOnBounds(transform.position), transform.position);
			//other.GetComponent<Enemy>().getDamage(damage * (1 - (distance / radius)));
			//print("distance: " + distance + ", radius: " + radius + ", percentage: " + (1 - (distance / radius)) + ", damage: " + damage * (1 - (distance / radius)));

            other.GetComponent<Rigidbody>().velocity = other.GetComponent<Rigidbody>().velocity * 0.95f;
            other.GetComponent<Rigidbody>().AddExplosionForce(explosionForce * Time.deltaTime, transform.position, radius, 0f);
        }
	}
}
