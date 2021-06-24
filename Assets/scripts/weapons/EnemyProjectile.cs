﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private MaterialPropertyBlock mpb;
    public ParticleSystemRenderer[] psRenderers;
    public GameObject explosion;

    protected void EPInitialize()
    {
        mpb = new MaterialPropertyBlock();
        StartCoroutine(FlashRed());
    }

    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
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

    public IEnumerator FlashRed()
    {
        while (true)
        {
            mpb.SetColor("_Color", new Color(1f, 0f, 0f, 1f));
            mpb.SetColor("_EmissionColor", new Color(1f, 0f, 0f, 1f) * 2f);
            foreach (ParticleSystemRenderer psr in psRenderers)
            {
                psr.SetPropertyBlock(mpb);
            }
            yield return new WaitForSeconds(0.1f);
            mpb.SetColor("_Color", new Color(1f, 0f, 0f, 0.04f));
            mpb.SetColor("_EmissionColor", new Color(1f, 0f, 0f, 0.02f));
            foreach (ParticleSystemRenderer psr in psRenderers)
            {
                psr.SetPropertyBlock(mpb);
            }
            yield return new WaitForSeconds(0.05f);
            mpb.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            mpb.SetColor("_EmissionColor", new Color(1f, 1f, 1f, 1f) * 4f);
            foreach (ParticleSystemRenderer psr in psRenderers)
            {
                psr.SetPropertyBlock(mpb);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
