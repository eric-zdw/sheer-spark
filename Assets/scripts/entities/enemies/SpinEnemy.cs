using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinEnemy : Enemy {
    public GameObject healthBar;
    public GameObject explosion;
    private GameObject bar;

    private Vector3 destination;
    public float cyclesPerSecond = 0.25f;
    
    void Start()
    {
        bar = Instantiate(healthBar);
        maxHealth = 100f;
        health = maxHealth;
        bar.GetComponent<HealthBar>().setTarget(gameObject);
    }

    void Update()
    {
        if (health <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(bar);
            Destroy(gameObject);
        }
    }

    void GetLocation()
    {

    }
}
