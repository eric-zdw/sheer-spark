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

    private float radius;

	private Vector3 direction;
	private GameObject player;
	private Vector3 mousePosition;

    // Use this for initialization
    void Start() {
        lifeTime = 5f;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * launchForce);
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag("Player");
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

		if (Input.GetButton("Fire1"))
			rb.AddForce(Vector3.Normalize((player.transform.position) - (transform.position)) * Time.deltaTime * 4000f);

		if (Input.GetButtonUp("Fire1")) {
			rb.velocity = new Vector3(0, 0, 0);
			mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11f));
			rb.AddForce(Vector3.Normalize(mousePosition - (transform.position)) * 2000f);
			//Destroy(gameObject, 1f);
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
			//other.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, radius * 2);
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
			Explode();
		}
	}


}
