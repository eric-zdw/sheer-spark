using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterProjectile : Projectile {

    public GameObject explosion;
    public GameObject explosion2;

    // Use this for initialization
    void Start() {
        projectileSpeed = 38f;
        lifeTime = 10f;
        damage = 9f;
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();
            other.gameObject.GetComponent<Enemy>().getDamage(damage);
        }
        else if (!other.gameObject.CompareTag("Player"))
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
