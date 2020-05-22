using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowWeapon2 : Weapon {

    public GameObject projectile;
    private Camera cam;
    private PlayerBehaviour player;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.2f;

    public float heatDamageRate = 0.005f;
    public float heatFireRate = 0.004f;
    public float heatRadiusRate = 0.002f;
    public float damage = 6f;
    public float radius = 1f;

    int altFire = 1;

    void Start () {
        SetFireRate(bFireRate);
        WeaponType weaponType = WeaponType.Automatic;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void Update()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f));
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
            float realDamage = damage * (1f + (heatDamageRate * player.getHeatFactor()));
            float realRadius = radius * (1f + (heatRadiusRate * player.getHeatFactor()));
            GameObject proj = Instantiate(
                projectile,
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.25f),
                Quaternion.Euler(0, 0, angle + Random.Range(-8f, 8f) + (altFire * 10f))
                );
            proj.GetComponent<YellowProjectile2>().setDamage(realDamage);
            proj.GetComponent<YellowProjectile2>().setRadius(realRadius);
            SetCooldown(bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        }
    }

    public override void Fire2()
    {
    }
}
