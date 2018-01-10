using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Utility : MonoBehaviour
{
    protected float useRate;
    protected float cooldown;
    protected float charges;

    public Utility()
    {
        useRate = 0f;
        cooldown = 0f;
    }

    public abstract void Activate();

    protected void SetUseRate(float ur)
    {
        useRate = ur;
    }

    protected float GetUseRate()
    {
        return useRate;
    }

    protected void SetCooldown(float cd)
    {
        cooldown = cd;
    }

    protected void DecrementCooldown()
    {
        cooldown -= Time.deltaTime;
    }

    protected float GetCooldown()
    {
        return cooldown;
    }
}
