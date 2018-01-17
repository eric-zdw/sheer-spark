using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeProjectile : Projectile {

    public GameObject explosion;

    public float extraGravityForce;
    public float launchForce;

    private Rigidbody rb;

    // Use this for initialization
    void Start() {
        lifeTime = 2.25f;
        damage = 40f;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * launchForce);
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
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Explode();
            if (other.gameObject.CompareTag("Enemy"))
                other.gameObject.GetComponent<Enemy>().getDamage(damage);
        }
    }
    */

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
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
