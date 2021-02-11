using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileHoming : Projectile {

    public GameObject explosion;

    private GameObject player;

    public float speed;

    private MaterialPropertyBlock mpb;
    private ParticleSystemRenderer psRenderer;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        projectileSpeed = speed;
        lifeTime = 8f;
        damage = 8f;

        mpb = new MaterialPropertyBlock();
        psRenderer = GetComponent<ParticleSystemRenderer>();

        StartCoroutine(FlashRed());
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            Quaternion originalRotation = transform.rotation;
            transform.right = player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(originalRotation, transform.rotation, 0.03f);
            //transform.rotation = Quaternion.RotateTowards(originalRotation, transform.rotation, 5f * Time.deltaTime);

            Propogate();
            lifeTime -= Time.deltaTime;
        }
        //Vector3.Slerp    
    }

    void OnTriggerEnter(Collider other)
    {
		print (other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            Explode();
			other.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
        }
        else if (!other.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public IEnumerator FlashRed() {
        while (true) {
            mpb.SetColor("_Color", new Color(1f, 0f, 0f, 1f));
            mpb.SetColor("_EmissionColor", new Color(1f, 0f, 0f, 1f) * 2f);
            psRenderer.SetPropertyBlock(mpb);
            yield return new WaitForSeconds(0.1f);
            mpb.SetColor("_Color", new Color(1f, 0f, 0f, 0.04f));
            mpb.SetColor("_EmissionColor", new Color(1f, 0f, 0f, 0.02f));
            psRenderer.SetPropertyBlock(mpb);
            yield return new WaitForSeconds(0.05f);
            mpb.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            mpb.SetColor("_EmissionColor", new Color(1f, 1f, 1f, 1f) * 4f);
            psRenderer.SetPropertyBlock(mpb);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
