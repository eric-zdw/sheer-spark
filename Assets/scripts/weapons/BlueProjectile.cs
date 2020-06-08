﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueProjectile : Projectile {

    public GameObject explosion;
    public GameObject hitbox;

    public float launchSpeed;
    private Vector3 launchVector;
    public float pullForce;

    private Rigidbody rb;
    private CameraFollow camfollow;
	private Camera cam;

    private float radius;

	private Vector3 direction;
	private GameObject player;
	private Vector3 mousePosition;

    private BlueWeapon bw;
    //private bool isCharged = false;
    //private bool isShot = false;
    private MeshRenderer mr;
    public Material chargeMaterial;
    public Material normalMaterial;
    //private float chargeTimer;
    //private float chargeMax = 0.75f;

    public float chargeCapacity = 5f;
    public float chargeRate = 2f;
    public float chargeScale = 1f;
    public float chargeMass = 10f;
    private float currentCharge = 0f;
    public float chargeDamage = 0f;

    private bool projectileMode = false;

    private int layermask = ~(1 << 9 | 1 << 13 | 1 << 8 | 1 << 14 | 1 << 18);


    // Use this for initialization
    void Start() {
        lifeTime = 8f;
        rb = GetComponent<Rigidbody>();
        projectileSpeed = launchSpeed;
        rb.AddForce(Random.insideUnitCircle * 2000f);
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag("Player");
        bw = GameObject.FindGameObjectWithTag("BlueWeapon").GetComponent<BlueWeapon>();
        mr = GetComponent<MeshRenderer>();
        //chargeTimer = chargeMax;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            lifeTime -= Time.deltaTime;
        }

        //isCharged = false;
        if (projectileMode == false) {
            if (Input.GetButton("Fire1"))
            {
                rb.AddForce(Vector3.Normalize((player.transform.position) - (transform.position)) * Time.deltaTime * pullForce * (rb.mass * 0.8f));
            }

            if (Input.GetButtonUp("Fire1")) {
                //isShot = true;
                //chargeTimer = chargeMax;
                //rb.velocity = new Vector3(0, 0, 0);
                GetComponent<SphereCollider>().isTrigger = true;
                mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f));
                launchVector = Vector3.Normalize(mousePosition - (transform.position)) * launchSpeed;
                Destroy(rb);
                projectileMode = true;
            }
        }
        else if (projectileMode == true) {
            CheckLinecastCollision();
        }

        /*
        if (chargeTimer > 0f)
        {
            if (!isCharged)
                isCharged = true;
            chargeTimer -= Time.deltaTime;
        }
        else if (chargeTimer <= 0 && isShot == true)
        {   
            if (isCharged)
            {
                isCharged = false;
            }
            isShot = true;
        }

        if (isCharged && ps.enableEmission == false)
        {
            ps.enableEmission = true;
        }
        else if (!isCharged && ps.enableEmission == true)
        {
            ps.enableEmission = false;
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag("Enemy") && projectileMode == false)
        {
			Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.CompareTag("Enemy") && projectileMode == false)
        {
            /*
			Instantiate(explosion, transform.position, transform.rotation);
			noiseManager.AddNoise(2f);
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, radius * 2);
			collision.gameObject.GetComponent<Enemy>().getDamage(damage);
			//Destroy(gameObject);
            */
            Explode();
        }
    }

    void CheckLinecastCollision() {
        RaycastHit info;
        if (Physics.Linecast(transform.position, transform.position + launchVector * Time.deltaTime, out info, layermask)) {
            transform.position = info.point;
            transform.position = info.point;
            if (info.collider.gameObject.CompareTag("Enemy")) {
                //info.collider.gameObject.GetComponent<Enemy>().getDamage(damage);
                Camera.main.GetComponent<CameraFollow>().AddNoise(2f);
            }
            Explode();
        }
        else
            transform.position += launchVector * Time.deltaTime;
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
        Camera.main.GetComponent<CameraFollow>().AddNoise(2f);

        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        exp.transform.localScale = new Vector3(radius * 0.5f, radius * 0.5f, radius * 0.5f);

        OrangeProjectileHitbox hb = Instantiate(hitbox, transform.position, Quaternion.identity).GetComponent<OrangeProjectileHitbox>();
        hb.GetComponent<SphereCollider>().radius = radius * 1.5f;
        hb.setDamage(damage);
        hb.setRadius(radius * 1.5f);
        hb.printRadius();

        GameObject.Destroy(transform.GetChild(0).gameObject, 1f);
        transform.GetChild(0).parent = null;

        Destroy(gameObject);
    }


}
