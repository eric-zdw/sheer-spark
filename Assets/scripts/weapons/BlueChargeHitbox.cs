using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueChargeHitbox : MonoBehaviour {

	private float damage;
    private float radius;
    public GameObject explosion;
    

    public float explosionForce;

	// Use this for initialization
	void Start () {
        
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
			other.GetComponent<Enemy>().getDamage(damage);
            other.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, 50f, 0f);
            Instantiate(explosion, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
        }
	}
}
