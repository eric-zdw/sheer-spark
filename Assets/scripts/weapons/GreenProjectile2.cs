using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenProjectile2 : Projectile
{
    GreenWeapon2 weapon;
    public GameObject puppetEffect;

    // Use this for initialization
    void Start()
    {
        lifeTime = 3f;
        weapon = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<GreenWeapon2>();
        GetComponent<Rigidbody>().AddForce(transform.right * 1000f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Propogate();
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            lifeTime -= Time.deltaTime;
        }
    }

    public void setDamage(float d)
    {
        damage = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            weapon.heldEnemy = other.gameObject;
            other.gameObject.layer = 22;
            other.attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            //Instantiate(puppetEffect, other.transform.position, Quaternion.identity, other.transform);
            weapon.holdingEnemy = true;
            Destroy(gameObject);
        }
    }
}
