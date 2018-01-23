using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleProjectile : Projectile {
    
    private float exponent;
    private float starting;

    void Start()
    {
        Destroy(gameObject, 3f);
        projectileSpeed = 250f;
    }
    
    void FixedUpdate()
    {
        Propogate();
    }


}
