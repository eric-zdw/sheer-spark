using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeWeapon : Weapon {

    public GameObject projectile;
    public GameObject projectile2;
    private Camera cam;
    private PlayerBehaviour player;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.16f;

    public float heatDamageRate = 0.005f;
    public float heatFireRate = 0.001f;
    public float heatRadiusRate = 0.002f;
    public float damage = 35f;
    public float radius = 5f;

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

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
    }

    public override void Fire1()
    {
        print(
            "Current heat factor: " + player.getHeatFactor()
            + " , damage: " + damage * (1f + (heatDamageRate * player.getHeatFactor()))
            + " , fire rate: " + bFireRate / (1f + (heatFireRate * player.getHeatFactor()))
            + " , radius: " + radius * (1f + (heatRadiusRate * player.getHeatFactor())));

        if (GetCooldown() <= 0)
        {
            float realDamage = damage * (1f + (heatDamageRate * player.getHeatFactor()));
            float realRadius = radius * (1f + (heatRadiusRate * player.getHeatFactor()));
            OrangeProjectile proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle)).GetComponent<OrangeProjectile>();
            proj.setDamage(realDamage);
            proj.setRadius(realRadius);
            SetCooldown(bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        }
    }

    public override void Fire2()
    {
    }
}
