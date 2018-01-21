using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeProjectileHitbox : MonoBehaviour {

	public float damage;
    private float radius = 4f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy") {
            float distance = Vector3.Distance(other.ClosestPointOnBounds(transform.position), transform.position);
			other.GetComponent<Enemy>().getDamage(damage * (1 - (distance / radius)));
			print("distance: " + distance + ", radius: " + radius + ", percentage: " + (1 - (distance / radius)) + ", damage: " + damage * (1 - (distance / radius)));
		}
	}
}
