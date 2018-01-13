using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollEnemy : Enemy {
    public GameObject healthBar;
    public GameObject explosion;
    public float newHealth;
    private GameObject bar;
    
    void Start()
    {
        bar = Instantiate(healthBar);
        maxHealth = newHealth;
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
}
