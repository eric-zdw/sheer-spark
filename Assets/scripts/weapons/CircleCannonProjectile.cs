using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCannonProjectile : Projectile {

    public GameObject explosion;
    public GameObject explosion2;

    // Use this for initialization
    void Start() {
        projectileSpeed = 8f;
        lifeTime = 10f;
        damage = 8f;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            Propogate();
            lifeTime -= Time.deltaTime;
        }
            
    }

    void OnTriggerEnter(Collider other)
    {
		print (other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            Explode();
			other.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
        }
        else if (!other.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Instantiate(explosion2, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
