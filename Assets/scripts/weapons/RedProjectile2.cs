using System.Collections;
using System.Linq;
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

    private float dampVelocity = 0f;
    private float slowRateRef = 0f;
    private float slowRate = 0f;

    private bool wasDestroyed = false;
    private List<int> enemiesHit;

    // Use this for initialization
    void Start() {
        projectileSpeed = Random.Range(50f, 400f);
        lifeTime = 0.25f;
        var particle = GetComponent<ParticleSystem>();
        var mainModule = particle.main;
        mainModule.startLifetime = lifeTime;
        collider = GetComponent<BoxCollider>();
        humSound = GetComponent<AudioSource>();
        humSound.volume = 0f;
        enemiesHit = new List<int>();

        //StartCoroutine(IncreaseSpeed());
        slowRate = 0.08f;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)

            Destroy(gameObject, 0.4f);
        else
        {
            CheckLinecastCollision();
            //damage = Mathf.SmoothDamp(damage, 0f, ref dampVelocity, 0.2f);
            projectileSpeed = Mathf.SmoothDamp(projectileSpeed, 0f, ref slowRateRef, slowRate);
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
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.1f, transform.right, Vector3.Magnitude(transform.right * projectileSpeed * Time.deltaTime), PlayerBehaviour.projectileLayerMask);
        hits = hits.OrderBy(x => Vector3.Distance(transform.position, x.point)).ToArray();
        foreach (RaycastHit info in hits) {
            if (!wasDestroyed) {
                //transform.position = info.point;
                if (info.collider.gameObject.CompareTag("Enemy") && !enemiesHit.Contains(info.collider.gameObject.GetInstanceID())) {
                    //each enemy can only be hit once
                    enemiesHit.Add(info.collider.gameObject.GetInstanceID());
                    float leftoverDamage = Mathf.Clamp(-(info.collider.gameObject.GetComponent<Enemy>().getHealth() - damage), 0f, damage);
                    //print("enemy health: " + info.collider.gameObject.GetComponent<Enemy>().getHealth() + ", damage: " + damage + ", leftoverDamage: " + leftoverDamage);
                    bool wasAlreadyDead = false;
                    if (info.collider.gameObject.GetComponent<Enemy>().getHealth() > 0) {
                        wasAlreadyDead = true;
                    }
                    info.collider.gameObject.GetComponent<Enemy>().getDamage(damage);
                    Explode(info.point);
                    if (!wasAlreadyDead) {
                        damage *= 0.6f;
                    }
                    Camera.main.GetComponent<CameraFollow>().AddNoise(damage * 0.2f);
                    // Pierce through enemies if full damage wasn't dealt
                    /*
                    if (leftoverDamage > 0f) {
                        damage = leftoverDamage;
                    }
                    else {
                        Explode();
                    }
                    */
                }
                else {
                    Explode(info.point);
                    transform.GetChild(0).parent = null;
                    transform.GetChild(0).parent = null;
                    wasDestroyed = true;
                    Destroy(gameObject);
                }
            }
        }
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

    void Explode(Vector3 point)
    {
        Camera.main.GetComponent<CameraFollow>().AddNoise(.5f);
        Instantiate(explosion, point, transform.rotation);
        Instantiate(explosion2, point, transform.rotation);
        Instantiate(explosion3, point, transform.rotation);
        //transform.GetChild(0).parent = null;
        //transform.GetChild(0).parent = null;
        
    }


}
