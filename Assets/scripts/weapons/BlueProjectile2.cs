using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueProjectile2 : Projectile {

	public GameObject explosion;
	public GameObject explosion2;

	private List<Collider> currentHits;

	private float damageDecayRate;
	private CapsuleCollider collider;


	// Use this for initialization
	void Start() {
		projectileSpeed = 30f;
		lifeTime = 5f;
		collider = GetComponent<CapsuleCollider>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (lifeTime <= 0)
			Destroy(gameObject);
		else
		{
			transform.position += -transform.up * projectileSpeed * Time.deltaTime;
			lifeTime -= Time.deltaTime;
		}

		projectileSpeed -= 25f * Time.deltaTime;
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
