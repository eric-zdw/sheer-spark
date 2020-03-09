using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueProjectile2 : Projectile {

	public GameObject explosion;
	public GameObject explosion2;

	private List<Collider> currentHits;

	private float damageDecayRate;
	private SphereCollider collider;

	private Rigidbody rb;

	private float projectileForce;

	private GameObject player;


	// Use this for initialization
	void Start() {
		projectileForce = -300f;
		lifeTime = 2f;
		collider = GetComponent<SphereCollider>();
		rb = GetComponent<Rigidbody>();
		rb.AddForce(-transform.up * 8000f * Time.deltaTime);
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (lifeTime <= 0)
			Destroy(gameObject);
		else
		{
			//transform.position += -transform.up * projectileForce * Time.deltaTime;
			//rb.AddForce(-transform.up * projectileForce * Time.deltaTime);
			rb.AddForce(Vector3.Normalize(transform.position - player.transform.position) * projectileForce * Time.deltaTime);
			lifeTime -= Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			Vector3 expPosition = collider.ClosestPointOnBounds(other.transform.position);
			Instantiate(explosion, expPosition, transform.rotation);
			//Instantiate(explosion2, expPosition, transform.rotation);
			other.gameObject.GetComponent<Enemy>().getDamage(damage);
			print("real damage: " + damage);
			damage *= 0.6f;
		}

	}

	private void OnTriggerStay(Collider other)
	{
		if (!other.gameObject.CompareTag("Player"))
		{
			Instantiate(explosion2, transform.position, transform.rotation);
		}
	}

	public void setDamage(float d)
	{
		damage = d;
		//damageDecayRate = damage * 0.5f;
	}

	void Explode()
	{
		Instantiate(explosion, transform.position, transform.rotation);
		Instantiate(explosion2, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	void DamageRoutine()
	{
		//foreach(Collider 
	}


}
