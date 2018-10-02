using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueProjectile : Projectile {

    public GameObject explosion;
    public GameObject hitbox;

    public float extraGravityForce;
    public float launchForce;

    private Rigidbody rb;
    private CameraFollow camfollow;
	private Camera cam;
	private NoiseManager noiseManager;

    private float radius;

	private Vector3 direction;
	private GameObject player;
	private Vector3 mousePosition;

    private BlueWeapon bw;
    private bool isCharged = false;
    private bool isShot = false;
    private MeshRenderer mr;
    public Material chargeMaterial;
    public Material normalMaterial;

    private ParticleSystem ps;
    private float chargeTimer;
    private float chargeMax = 0.75f;

    // Use this for initialization
    void Start() {
        lifeTime = 3f;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * launchForce);
		noiseManager = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<NoiseManager>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag("Player");
        bw = GameObject.FindGameObjectWithTag("BlueWeapon").GetComponent<BlueWeapon>();
        mr = GetComponent<MeshRenderer>();
        ps = GetComponent<ParticleSystem>();
        ps.enableEmission = false;
        chargeTimer = chargeMax;
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
        rb.AddForce(new Vector3(0, -extraGravityForce, 0) * Time.deltaTime);

        isCharged = false;
        if (Input.GetButton("Fire1"))
        {
            rb.AddForce(Vector3.Normalize((player.transform.position) - (transform.position)) * Time.deltaTime * 80f);
            if (Vector3.Magnitude(bw.gameObject.transform.position - transform.position) < bw.rangeRadius)
            {
                isCharged = true;
            }
        }

		if (Input.GetButtonUp("Fire1")) {
            if (Vector3.Magnitude(bw.gameObject.transform.position - transform.position) < bw.rangeRadius)
            {
                isShot = true;
                chargeTimer = chargeMax;
                rb.velocity = new Vector3(0, 0, 0);
                mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11f));
                rb.AddForce(Vector3.Normalize(mousePosition - (transform.position)) * 60f);
            }
			//Destroy(gameObject, 1f);
		}

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
			collision.gameObject.GetComponent<Enemy>().getDamage(damage);
			Destroy(gameObject);
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
