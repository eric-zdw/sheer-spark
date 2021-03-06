﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour{

    public enum WeaponType {Automatic, SingleShot};
    private float fireRate;
    private float cooldown;

    public Weapon()
    {
        WeaponType weaponType = WeaponType.Automatic;
        fireRate = 0f;
        cooldown = 0f;
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

    public abstract void Fire1();
    public abstract void Fire2();
}
