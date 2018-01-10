using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    protected float health;
    protected float maxHealth;
    
    public Enemy()
    {
        health = 100f;
    }

    public void getDamage(float damage)
    {
        health -= damage;
    }

    public void setHealth(float h)
    {
        health = h;
    }

    public float getHealth()
    {
        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }
}
