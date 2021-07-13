﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowWeapon : Weapon {

    public GameObject projectile;
    private Camera cam;
    private PlayerBehaviour player;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.2f;

    public float maxHeatDamageMulti = 0.5f;
    public float maxHeatFireRateMulti = 1f;
    public float maxHeatRadiusMulti = 1f;
    public float damage = 6f;
    public float radius = 1f;

    int altFire = 1;

    void Start () {
        SetFireRate(bFireRate);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void FixedUpdate()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraFollow.CameraDistance));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
    }

    public override void Fire1()
    {
        /*print(
            "Current heat factor: " + player.getHeatFactor()
            + " , damage: " + damage * (1f + (heatDamageRate * player.getHeatFactor()))
            + " , fire rate: " + bFireRate / (1f + (heatFireRate * player.getHeatFactor())));*/
        if (GetCooldown() <= 0)
        {
            altFire *= -1;
            float realDamage = damage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(EnergyColor.Yellow)));
            float realRadius = radius * (1f + (maxHeatRadiusMulti * player.GetHeatFactor(EnergyColor.Yellow)));
            GameObject proj = Instantiate(
                projectile,
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 2f),
                Quaternion.Euler(0, 0, angle + Random.Range(-2f, 2f) + (altFire * 0f))
                );
            proj.GetComponent<YellowProjectile>().setDamage(realDamage);
            proj.GetComponent<YellowProjectile>().setRadius(realRadius);
            player.GetComponent<Rigidbody>().AddForce(Vector3.Normalize((Vector3)mousePosition - transform.position) * -12f);
            SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Yellow))));
        }
    }

    public override void Fire2()
    {
    }
}
