using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterProjectile : Projectile {

    public GameObject explosion;
    public GameObject explosion2;

    // Use this for initialization
    void Start() {
        projectileSpeed = 120f;
        lifeTime = 10f;
        damage = 11f;
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
        if (Physics.Linecast(transform.position, transform.position + transform.right * projectileSpeed * Time.deltaTime, out info, PlayerBehaviour.projectileLayerMask)) {
            transform.position = info.point;
            if (info.collider.gameObject.CompareTag("Enemy")) {
                info.collider.gameObject.GetComponent<Enemy>().getDamage(damage);
            }
            Explode();
        }
        else
            transform.position += transform.right * projectileSpeed * Time.deltaTime;
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Instantiate(explosion2, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
