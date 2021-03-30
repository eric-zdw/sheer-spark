using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedProjectile1 : Projectile {

    public GameObject explosion;
    public GameObject explosion2;
    public GameObject explosion3;

    public float damageDecayRate = 20f;
    public float minimumDamage = 2f;
    private BoxCollider collider;

    private AudioSource humSound;
    private bool soundDecreasing = false;

    public GameObject burnPrefab;

    private int layermask = ~(1 << 9 | 1 << 13 | 1 << 8 | 1 << 14 | 1 << 2 | 1 << 18);

    // Use this for initialization
    void Start() {
        projectileSpeed = 200f;
        projectileSpeed = Random.Range(projectileSpeed * 0.5f, projectileSpeed * 1.5f);
        lifeTime = 1.4f;
        collider = GetComponent<BoxCollider>();
        humSound = GetComponent<AudioSource>();
        humSound.volume = 0f;

        GetComponent<Rigidbody>().AddForce(transform.right * projectileSpeed);

        StartCoroutine(DecreaseSpeed());
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            lifeTime -= Time.deltaTime;
        }

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

    IEnumerator DecreaseSpeed() {
        while(true) {
            //projectileSpeed *= .70f;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<Enemy>().getDamage(damage);
            
            if (collision.transform.Find("BurnEffect"))
            {
                BurnEffect b = collision.transform.Find("BurnEffect").GetComponent<BurnEffect>();
                b.burnStacks += 3;
            }
            else
            {
                GameObject burnObj = Instantiate(burnPrefab, collision.transform.position, Quaternion.identity, collision.transform);
                burnObj.name = "BurnEffect";
            }
            
            //Camera.main.GetComponent<CameraFollow>().AddNoise(1f);
            Explode(transform.position);
        }
    }

    /*
    void CheckLinecastCollision() {
        RaycastHit info;
        if (Physics.Linecast(transform.position, transform.position + transform.right * projectileSpeed * Time.deltaTime, out info, layermask)) {
            //transform.position = info.point;
            if (info.collider.gameObject.CompareTag("Enemy")) {
                info.collider.gameObject.GetComponent<Enemy>().getDamage(damage);
                damage *= 0.8f;
                Camera.main.GetComponent<CameraFollow>().AddNoise(1f);
            }
            if (!info.collider.gameObject.CompareTag("Enemy")) {
                Explode(info.point);
            } else ExplodeWithoutDestroy(info.point);
        }
        transform.position += transform.right * projectileSpeed * Time.fixedDeltaTime;
    }
    */

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

    void Explode(Vector3 pos)
    {
        Camera.main.GetComponent<CameraFollow>().AddNoise(1f);
        //Instantiate(explosion, pos, transform.rotation);
        //Instantiate(explosion2, pos, transform.rotation);
        //Instantiate(explosion3, pos, transform.rotation);
        transform.GetChild(0).parent = null;
        Destroy(gameObject);
    }
    
    void ExplodeWithoutDestroy(Vector3 pos)
    {
        Camera.main.GetComponent<CameraFollow>().AddNoise(1f);
        Instantiate(explosion, pos, transform.rotation);
        Instantiate(explosion2, pos, transform.rotation);
        Instantiate(explosion3, pos, transform.rotation);
    }    


}
