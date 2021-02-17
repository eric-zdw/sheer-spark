using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeWeapon2 : Weapon {

    public GameObject projectile;
    public GameObject projectile2;
    private Camera cam;
    private PlayerBehaviour player;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.16f;
    public float maxHeatDamageMulti = 0.5f;
    public float maxHeatFireRateMulti = 1f;
    public float maxHeatRadiusMulti = 1f;
    public float damage = 35f;
    public float radius = 5f;

    void Start () {
        SetFireRate(bFireRate);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void Update()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraFollow.CameraDistance));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
    }

    public override void Fire1()
    {
        if (GetCooldown() <= 0)
        {
            float realDamage = damage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(EnergyColor.Orange)));
            float realRadius = radius * (1f + (maxHeatRadiusMulti * player.GetHeatFactor(EnergyColor.Orange)));
            OrangeProjectile2 proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle + Random.Range(-15f, 15f))).GetComponent<OrangeProjectile2>();
            proj.setDamage(realDamage);
            proj.setRadius(realRadius);
            SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Orange))));
        }
    }

    public override void Fire2()
    {
    }
}
