using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueProjectile : Projectile {

    public GameObject explosion;
    public GameObject hitbox;

    public float launchForce;
    public float pullForce;

    private Rigidbody rb;
    private CameraFollow camfollow;
	private Camera cam;
	private NoiseManager noiseManager;

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

    private ParticleSystem ps;
    //private float chargeTimer;
    //private float chargeMax = 0.75f;

    public float chargeCapacity = 5f;
    public float chargeRate = 2f;
    public float chargeScale = 1f;
    public float chargeMass = 10f;
    private float currentCharge = 0f;
    public float chargeDamage = 0f;


    // Use this for initialization
    void Start() {
        lifeTime = 6.5f;
        rb = GetComponent<Rigidbody>();
        rb.mass = 0.01f;
        //rb.AddForce(transform.right * launchForce);
		noiseManager = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<NoiseManager>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag("Player");
        bw = GameObject.FindGameObjectWithTag("BlueWeapon").GetComponent<BlueWeapon>();
        mr = GetComponent<MeshRenderer>();
        ps = GetComponent<ParticleSystem>();
        ps.enableEmission = true;
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

		CheckDetonate();

        //isCharged = false;
        if (Input.GetButton("Fire1"))
        {
            rb.AddForce(Vector3.Normalize((player.transform.position) - (transform.position)) * Time.deltaTime * pullForce * (rb.mass * 0.8f));
            if (Vector3.Magnitude(bw.gameObject.transform.position - transform.position) < bw.rangeRadius)
            {
                //transform.localScale += new Vector3(0.5f * Time.deltaTime, 0.5f * Time.deltaTime, 0.5f * Time.deltaTime);
                currentCharge += chargeRate * (chargeCapacity - currentCharge) * Time.deltaTime;
                print("charge: " + currentCharge);
                float newScale = currentCharge * chargeScale;
                transform.localScale = new Vector3(newScale, newScale, newScale);
                rb.mass = currentCharge * chargeMass;
                damage = currentCharge * chargeDamage;
                print("mass: " + rb.mass);
            }
        }

		if (Input.GetButtonUp("Fire1")) {
            if (Vector3.Magnitude(bw.gameObject.transform.position - transform.position) < bw.rangeRadius)
            {
                //isShot = true;
                //chargeTimer = chargeMax;
                rb.velocity = new Vector3(0, 0, 0);
                mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f));
                rb.AddForce(Vector3.Normalize(mousePosition - (transform.position)) * launchForce * (rb.mass * 0.8f));
            }
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

    /*
    private void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("Enemy"))
        {
			Instantiate(explosion, transform.position, transform.rotation);
			//other.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, radius * 2);
			other.gameObject.GetComponent<Enemy>().getDamage(damage);
			Destroy(gameObject);
        }
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.CompareTag("Enemy"))
        {
			Instantiate(explosion, transform.position, transform.rotation);
			noiseManager.AddNoise(2.5f);
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, radius * 2);
			collision.gameObject.GetComponent<Enemy>().getDamage(damage);
			//Destroy(gameObject);
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
			//Explode();
		}
	}


}
