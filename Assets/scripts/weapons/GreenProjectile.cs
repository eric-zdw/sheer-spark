﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenProjectile : Projectile {

    public GameObject explosion;
    public GameObject hitbox;

    public float extraGravityForce;
    public float launchForce;

    private Rigidbody rb;
    private CameraFollow cam;

    private float radius;
	private float cooldown = 0.0f;

	private int charges = 5;

    // Use this for initialization
    void Start() {
        lifeTime = 8f;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * launchForce);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Explode();
        else
        {
            lifeTime -= Time.deltaTime;
        }

		CheckDetonate();
        rb.AddForce(new Vector3(0, -extraGravityForce, 0) * Time.deltaTime);

		if (cooldown > 0f)
			cooldown -= Time.deltaTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.CompareTag("Enemy") && cooldown <= 0f)
        {
			Instantiate(explosion, transform.position, transform.rotation, transform);
			collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(2000f, transform.position, radius * 2);
			collision.gameObject.GetComponent<Enemy>().getDamage(damage);
			charges -= 1;

			if (charges == 0)
				Destroy(gameObject);

			cooldown = 0.2f;
        }
    }

    public void setDamage(float d)
    {
        damage = d;
    }

    public void setRadius(float r)
    {
        radius = r;
    }

    void Explode()
    {
        cam.addShake(0.25f);

        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        exp.transform.localScale = new Vector3(radius * 0.5f, radius * 0.5f, radius * 0.5f);

        OrangeProjectileHitbox hb = Instantiate(hitbox, transform.position, Quaternion.identity).GetComponent<OrangeProjectileHitbox>();
        hb.GetComponent<SphereCollider>().radius = radius;
        hb.setDamage(damage);
        hb.setRadius(radius / 0.5f);
        hb.printRadius();

        Destroy(gameObject);
    }

	void CheckDetonate()
	{
		if (Input.GetButtonDown("Fire2"))
		{
			Explode();
		}
	}


}
