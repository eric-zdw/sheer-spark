using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleArrow : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.right * 120f, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.right = GetComponent<Rigidbody>().velocity;
    }
    public void setDamage(float d)
    {
        damage = d;
    }
}
