using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedProjectile2 : Projectile {

    public GameObject explosion;
    public GameObject explosion2;
    public GameObject explosion3;

    //private float damageDecayRate;
    private BoxCollider collider;

    private AudioSource humSound;
    private bool soundDecreasing = false;

    private int layermask = ~(1 << 9 | 1 << 13 | 1 << 8 | 1 << 14 | 1 << 18);

    // Use this for initialization
    void Start() {
        projectileSpeed = 200f;
        lifeTime = 2f;
        collider = GetComponent<BoxCollider>();
        humSound = GetComponent<AudioSource>();
        humSound.volume = 0f;

        //StartCoroutine(IncreaseSpeed());
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            CheckLinecastCollision();
            //Propogate();
            lifeTime -= Time.deltaTime;
        }

        //damage -= damageDecayRate * Time.deltaTime;
        //transform.localScale = new Vector3(transform.localScale.x * 1.25f, transform.localScale.y, transform.localScale.z);

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

    IEnumerator IncreaseSpeed() {
        while(true) {
            projectileSpeed *= 1.5f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void CheckLinecastCollision() {
        RaycastHit info;
        if (Physics.Linecast(transform.position, transform.position + transform.right * projectileSpeed * Time.deltaTime, out info, layermask)) {
            transform.position = info.point;
            if (info.collider.gameObject.CompareTag("Enemy")) {
                info.collider.gameObject.GetComponent<Enemy>().getDamage(damage);
                Camera.main.GetComponent<CameraFollow>().AddNoise(.75f);
            }
            Explode();
        }
        else
            transform.position += transform.right * projectileSpeed * Time.deltaTime;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector3 expPosition = collider.ClosestPointOnBounds(other.transform.position);
            //Instantiate(explosion, expPosition, transform.rotation);
            //Instantiate(explosion2, expPosition, transform.rotation);
            other.gameObject.GetComponent<Enemy>().getDamage(damage);
            print("real damage: " + damage);
            damage *= 0.6f;
            Explode();
        }
        else if (!other.gameObject.CompareTag("Player"))
        {
            Vector3 expPosition = collider.ClosestPointOnBounds(other.transform.position);
            //Instantiate(explosion, expPosition, transform.rotation);
            //Instantiate(explosion2, expPosition, transform.rotation);
            damage *= 0.5f;
            Explode();
        }
    }
    */

    public void setDamage(float d)
    {
        damage = d;
        //damageDecayRate = damage * 0.5f;
    }

    void Explode()
    {
        Camera.main.GetComponent<CameraFollow>().AddNoise(.5f);
        Instantiate(explosion, transform.position, transform.rotation);
        Instantiate(explosion2, transform.position, transform.rotation);
        Instantiate(explosion3, transform.position, transform.rotation);
        transform.GetChild(0).parent = null;
        transform.GetChild(0).parent = null;
        Destroy(gameObject);
    }


}
