﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : Weapon {

    public GameObject projectile;
    public GameObject projectile2;
    private Camera cam;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.18f;
    public float secondaryRate = 0.90f;

    public float maxHeatDamageMulti = 1f;
    public float maxHeatFireRateMulti = 1f;
    //public float maxHeatRadiusMulti = 1f;

    void Start () {
        //SetFireRate(bFireRate);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(CameraFollow.CameraDistance)));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
    }

    public override void Fire1()
    {
        if (GetCooldown() <= 0)
        {
            Instantiate(projectile, transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.5f), Quaternion.Euler(0, 0, angle + Random.Range(-1f, 1f)));
            //SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.White))));
            SetCooldown(bFireRate);
        }
    }

    public override void Fire2()
    {
    }
}
