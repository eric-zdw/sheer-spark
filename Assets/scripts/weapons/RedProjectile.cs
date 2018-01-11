﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedProjectile : Projectile {

    public GameObject explosion;
    public GameObject explosion2;

    // Use this for initialization
    void Start() {
        projectileSpeed = 38f;
        lifeTime = 0.2f;
        damage = 6f;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();
            other.gameObject.GetComponent<Enemy>().getDamage(damage);
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Instantiate(explosion2, transform.position, transform.rotation);
        Destroy(gameObject);
    }


}