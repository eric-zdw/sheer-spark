using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeProjectileExplosion : MonoBehaviour {

	public float damage;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		print("detected!");
		if (other.tag == "Enemy") {
			other.GetComponent<Enemy>().getDamage(damage);
			print("enemy detected!");
		}
	}
}
