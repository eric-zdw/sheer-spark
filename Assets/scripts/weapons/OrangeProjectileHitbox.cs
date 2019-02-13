using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeProjectileHitbox : MonoBehaviour {

	private float damage;
    private float radius;

    public float explosionForce;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setDamage(float d)
    {
        damage = d;
    }

    public void setRadius(float r)
    {
        radius = r;
        GetComponent<SphereCollider>().radius = radius;
    }

    public void printRadius()
    {
        print("radius: " + radius + ", damage: " + damage);
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy") {
            float distance = Vector3.Distance(other.ClosestPointOnBounds(transform.position), transform.position);
			other.GetComponent<Enemy>().getDamage(damage * (1 - (distance / radius)));
			print("distance: " + distance + ", radius: " + radius + ", percentage: " + (1 - (distance / radius)) + ", damage: " + damage * (1 - (distance / radius)));

            other.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, radius, 0f);
        }
	}
}
