using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile1 : Projectile {

    public GameObject explosion;

    public float speed;

    private MaterialPropertyBlock mpb;
    private ParticleSystemRenderer renderer;

    // Use this for initialization
    void Start() {
        projectileSpeed = speed;
        lifeTime = 8f;
        damage = 8f;

        mpb = new MaterialPropertyBlock();
        renderer = GetComponent<ParticleSystemRenderer>();

        StartCoroutine(FlashRed());
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

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public IEnumerator FlashRed() {
        while (true) {
            mpb.SetColor("_Color", new Color(1f, 0f, 0f, 1f));
            mpb.SetColor("_EmissionColor", new Color(1f, 0f, 0f, 1f) * 2f);
            renderer.SetPropertyBlock(mpb);
            yield return new WaitForSeconds(0.1f);
            mpb.SetColor("_Color", new Color(1f, 0f, 0f, 0.04f));
            mpb.SetColor("_EmissionColor", new Color(1f, 0f, 0f, 0.02f));
            renderer.SetPropertyBlock(mpb);
            yield return new WaitForSeconds(0.05f);
            mpb.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            mpb.SetColor("_EmissionColor", new Color(1f, 1f, 1f, 1f) * 4f);
            renderer.SetPropertyBlock(mpb);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
