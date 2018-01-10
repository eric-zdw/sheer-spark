using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    protected float projectileSpeed;
    protected float lifeTime;
    protected float damage;

	public Projectile()
    {
        projectileSpeed = 0;
        lifeTime = 0;
        damage = 0;
    }

    public virtual void Propogate()
    {
        transform.position += transform.right * projectileSpeed * Time.deltaTime;
    }

    public virtual void End()
    {
        Destroy(gameObject);
    }

}
