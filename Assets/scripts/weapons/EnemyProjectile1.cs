using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile1 : EnemyProjectile {

    //public GameObject explosion;

    public float speed;

    // Use this for initialization
    void Start() {
        projectileSpeed = speed;
        lifeTime = 8f;
        damage = 8f;

        EPInitialize();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            Propogate();
            lifeTime -= Time.deltaTime;
        }
            
    }

    /*
    void OnTriggerEnter(Collider other)
    {
		print (other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            Explode();
			other.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
        }
        else if (other.gameObject.CompareTag("Geometry"))
        {
            Explode();
        }
    }
    */

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
