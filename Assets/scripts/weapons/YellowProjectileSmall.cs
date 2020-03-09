using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowProjectileSmall : Projectile {

    public GameObject explosion;
    public GameObject hitbox;

    private Camera cam;
    private Vector3 mousePosition;
    private float radius;

    private float projectileSpeedIncrease = 0f;
    private int layermask = ~(1 << 9 | 1 << 13 | 1 << 8 | 1 << 14);

    //private float angle1;
    //private float angle2;
    
    // Use this for initialization
    void Start() {
        projectileSpeed = 100f;
        lifeTime = 3.5f;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
        else if (!(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Powerup")))
        {
            Explode();
        }
    }

    void CheckLinecastCollision() {
        RaycastHit info;
        if (Physics.Linecast(transform.position, transform.position + transform.right * projectileSpeed * Time.deltaTime, out info, layermask)) {
            transform.position = info.point;
            if (info.collider.gameObject.CompareTag("Enemy")) {
                info.collider.gameObject.GetComponent<Enemy>().getDamage(damage);
                Camera.main.GetComponent<CameraFollow>().AddNoise(2f);
            }
            Explode();
        }
        else
            transform.position += transform.right * projectileSpeed * Time.deltaTime;
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
        hb.setRadius(radius / 0.25f);                //minimum damage is 1-x%
        hb.printRadius();

        ParticleSystem.EmissionModule emission = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
        emission.rateOverTime = 0f;
        Destroy(transform.GetChild(0).gameObject, 2f);
        transform.GetChild(0).parent = null;
        
        

        Destroy(gameObject);
    }


}
