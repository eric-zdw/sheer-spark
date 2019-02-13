﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenProjectile : Projectile {

    public GameObject explosion;
    public GameObject explosion2;
    public GameObject explosion3;
    public GameObject hitbox;
    public GameObject hitbox2;
    public GameObject tether;

    public float extraGravityForce;
    public float launchForce;

    private Rigidbody rb;
	private NoiseManager noiseManager;

    private float radius;
	private float cooldown = 0.0f;

	private int charges = 5;

    private float pulseTimer;
    private float pulseInterval = 0.05f;
    private float tetherCheck = 0.1f;
    private float tetherTimer;
    private float damageTimer;
    private float damageInterval = 1.1f;


    // Use this for initialization
    void Start() {
        lifeTime = 3.3f;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * launchForce);
		noiseManager = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<NoiseManager>();
        pulseTimer = pulseInterval;
        GameObject exp = Instantiate(explosion, transform.position, transform.rotation, transform);
        exp.transform.localScale = new Vector3(radius * 0.1f, radius * 0.1f, radius * 0.1f);
        damageTimer = damageInterval;

        StartCoroutine(PulseTimer());
        StartCoroutine(TetherCheck());
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Explode();
        else
        {
            lifeTime -= Time.deltaTime;
        }

        pulseTimer -= Time.deltaTime;
        if (pulseTimer <= 0f)
        {
            pulseTimer = pulseInterval;
            GreenProjectileHitbox hb = Instantiate(hitbox, transform.position, Quaternion.identity).GetComponent<GreenProjectileHitbox>();
            hb.GetComponent<SphereCollider>().radius = radius;
            hb.setDamage(0);
            hb.setRadius(radius / 0.5f);
            hb.printRadius();
        }

		CheckDetonate();
        rb.AddForce(new Vector3(0, -extraGravityForce, 0) * Time.deltaTime);

		if (cooldown > 0f)
			cooldown -= Time.deltaTime;
    }

    IEnumerator PulseTimer() {
        while (true) {
            yield return new WaitForSeconds(damageInterval);
            damageTimer = damageInterval;
            damagePulse();
        }
    }

    IEnumerator TetherCheck() {
        while (true) {
            Collider[] tethers = Physics.OverlapSphere(transform.position, radius);
            for (int i = 0; i < tethers.Length; i++)
            {
                if (tethers[i].tag == "Enemy")
                {
                    GameObject te = Instantiate(tether, tethers[i].transform.position, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(tetherCheck);
        }
    }

    private void damagePulse()
    {
        noiseManager.AddNoise(6f);
        GameObject exp = Instantiate(explosion2, transform.position, transform.rotation, transform);
        exp.transform.localScale = new Vector3(radius * 0.2f, radius * 0.2f, radius * 0.2f);

        OrangeProjectileHitbox hb = Instantiate(hitbox2, transform.position, Quaternion.identity).GetComponent<OrangeProjectileHitbox>();
        hb.GetComponent<SphereCollider>().radius = radius;
        hb.setDamage(damage * 0.125f);
        hb.setRadius(radius / 0.5f);
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
        noiseManager.AddNoise(15f);

        GameObject exp = Instantiate(explosion3, transform.position, transform.rotation);
        exp.transform.localScale = new Vector3(radius * 0.5f, radius * 0.5f, radius * 0.5f);

        OrangeProjectileHitbox hb = Instantiate(hitbox2, transform.position, Quaternion.identity).GetComponent<OrangeProjectileHitbox>();
        hb.GetComponent<SphereCollider>().radius = radius;
        hb.setDamage(damage * 0.75f);
        hb.setRadius(radius / 0.75f);

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
