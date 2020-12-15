using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeProjectile : Projectile {

    public GameObject explosion;
    public GameObject hitbox;

    public float extraGravityForce;
    public float launchForce;

    private Rigidbody rb;

    private float radius;

    // Use this for initialization
    void Start() {
        lifeTime = 2f;
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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //30% direct hit damage bonus?
            collision.gameObject.GetComponent<Enemy>().getDamage(damage * 0.3f);
            Explode();
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
        Camera.main.GetComponent<CameraFollow>().AddNoise(5f);

        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        exp.transform.localScale = new Vector3(radius * 0.35f, radius * 0.35f, radius * 0.35f);

        OrangeProjectileHitbox hb = Instantiate(hitbox, transform.position, Quaternion.identity).GetComponent<OrangeProjectileHitbox>();
        hb.GetComponent<SphereCollider>().radius = radius;
        hb.setDamage(damage);
        hb.setRadius(radius);
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
