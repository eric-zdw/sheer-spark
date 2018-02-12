using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedProjectile : Projectile {

    public GameObject explosion;
    public GameObject explosion2;

    private float damageDecayRate;

    // Use this for initialization
    void Start() {
        projectileSpeed = 50f;
        lifeTime = 0.15f;
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

        damage -= damageDecayRate * Time.deltaTime;
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();
            other.gameObject.GetComponent<Enemy>().getDamage(damage);
            print("real damage: " + damage);
        }
        else if (!other.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void setDamage(float d)
    {
        damage = d;
        damageDecayRate = (damage * 0.5f) * (1f / 0.15f);
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Instantiate(explosion2, transform.position, transform.rotation);
        Destroy(gameObject);
    }


}
