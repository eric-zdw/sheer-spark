using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour{

    private float fireRate;
    private float cooldown;
    public int energyCost;
    protected Vector3 mousePosition;

    public Weapon()
    {
        fireRate = 0f;
        cooldown = 0f;
    }
    protected void UpdateMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
    }

    protected void SetFireRate(float fr)
    {
        fireRate = fr;
    }

    protected float GetFireRate()
    {
        return fireRate;
    }

    protected void SetCooldown(float cd)
    {
        cooldown += cd;
    }

    protected void ResetCooldown()
    {
        cooldown = 0f;
    }

    protected void DecrementCooldown()
    {
        cooldown -= Time.deltaTime;
        if (cooldown < 0f)
        {
            cooldown = 0f;
        }
    }

    protected float GetCooldown()
    {
        return cooldown;
    }

    public abstract void Fire1();
    public abstract void Fire2();
}
