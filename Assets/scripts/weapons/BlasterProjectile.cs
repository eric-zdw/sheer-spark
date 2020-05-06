using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterProjectile : Projectile {

    public GameObject explosion;
    public GameObject explosion2;

    private int layermask = ~(1 << 9 | 1 << 13 | 1 << 8 | 1 << 14 | 1 << 18);

    // Use this for initialization
    void Start() {
        projectileSpeed = 70f;
        lifeTime = 10f;
        damage = 6f;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            //Propogate();
            CheckLinecastCollision();
            lifeTime -= Time.deltaTime;
        }
            
    }

    void CheckLinecastCollision() {
        RaycastHit info;
        if (Physics.Linecast(transform.position, transform.position + transform.right * projectileSpeed * Time.deltaTime, out info, layermask)) {
            transform.position = info.point;
            if (info.collider.gameObject.CompareTag("Enemy")) {
                info.collider.gameObject.GetComponent<Enemy>().getDamage(damage);
            }
            Explode();
        }
        else
            transform.position += transform.right * projectileSpeed * Time.deltaTime;
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
