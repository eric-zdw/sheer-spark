using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public float maxHealth;

    [SerializeField]
    protected int scoreValue;

    protected float health;
    
    protected virtual void Initialize()
    {
    }

    public virtual void getDamage(float damage)
    {
        health -= damage;
    }
    
    public virtual void setHealth(float h)
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
