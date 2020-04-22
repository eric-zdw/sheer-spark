using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeProjectile2 : Projectile {

    public GameObject explosion;
    public GameObject hitbox;

    public float extraGravityForce;
    public float launchForce;

    private Rigidbody rb;

    private float radius;
    private bool isStuck = false;

    // Use this for initialization
    void Start() {
        lifeTime = 3f;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * launchForce + transform.right * Random.Range(-200f, 200f));
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Explode();
        else
        {
            lifeTime -= Time.deltaTime;
        }

        if (isStuck == true && transform.parent == null) Explode();

		CheckDetonate();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(GetComponent<Rigidbody>());
            transform.SetParent(collision.transform);
            isStuck = true;
            //collision.gameObject.GetComponent<Enemy>().getDamage(damage * 0.25f);
            //Explode();
        }
    }

    public void OnDestroy() {
        Explode();
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
        hb.GetComponent<SphereCollider>().radius = radius * 1.5f;
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
