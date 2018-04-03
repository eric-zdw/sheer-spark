using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedProjectile : Projectile {

    public GameObject explosion;
    public GameObject explosion2;

    private float damageDecayRate;
    private BoxCollider collider;

    private AudioSource humSound;
    private bool soundDecreasing = false;

    // Use this for initialization
    void Start() {
        projectileSpeed = 2f;
        lifeTime = 0.5f;
        collider = GetComponent<BoxCollider>();
        humSound = GetComponent<AudioSource>();
        humSound.volume = 0f;
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

        damage -= damageDecayRate * Time.deltaTime;
        projectileSpeed *= 1.3f;
        transform.localScale = new Vector3(transform.localScale.x * 1.25f, transform.localScale.y, transform.localScale.z);

        if (soundDecreasing)
            humSound.volume -= 0.5f * Time.deltaTime;
        else
        {
            humSound.volume += 0.15f * Time.deltaTime;
            humSound.pitch += 0.1f * Time.deltaTime;
        }

        if (humSound.volume >= 0.05f)
            soundDecreasing = true;
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
        else if (!other.gameObject.CompareTag("Player"))
        {
            //Vector3 expPosition = collider.ClosestPointOnBounds(other.transform.position);
            //Instantiate(explosion, expPosition, transform.rotation);
            //Instantiate(explosion2, expPosition, transform.rotation);
            //damage *= 0.5f;
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


}
