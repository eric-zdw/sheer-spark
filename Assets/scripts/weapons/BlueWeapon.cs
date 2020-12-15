using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueWeapon : Weapon {
    public GameObject explosion;
    public GameObject explosionHitbox;
    public GameObject chargeHitbox;
    private Camera cam;
    private PlayerBehaviour player;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.2f;

    public float heatDamageRate = 0.005f;
    public float heatFireRate = 0.004f;
    public float damage = 6f;
    public float chargeDamage = 20f;

    public float rangeRadius = 0f;
    private float lastRR = 0f;

    public GameObject rangeSphere;
    private GameObject rs;
    private AudioSource[] sounds;

    public Material rangeMaterial;

    private BlueChargeHitbox chargeHit;

    public GameObject projectile;
    public float radius = 5f;
    public float heatRadiusRate = 0.002f;

    public float rotationSpeed = 2f;
    public float rotationDistance = 3f;

    static public float circleX = 0;
    static public float circleY = 0;
    static public int numberOfProjectiles = 0;
    public int maxProjectiles = 8;
    public int ringJumps = 1;
    public int spinDirection = 1;

    void Start () {
        SetFireRate(bFireRate);
        WeaponType weaponType = WeaponType.Automatic;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        rs = Instantiate(rangeSphere, transform.position, Quaternion.identity, transform);
        numberOfProjectiles = 0;
    }

    void Update()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraFollow.CameraDistance));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        rs.transform.position = transform.position;
        rs.transform.localScale = new Vector3(rangeRadius * (2f/0.3f), rangeRadius * (2f / 0.3f), rangeRadius * (2f / 0.3f));
        if (Input.GetButtonUp("Fire1"))
        {
            BlueWeapon.numberOfProjectiles = 0;
        }

        //Debug.DrawLine(transform.position, transform.position + new Vector3(rangeRadius, 0, 0));
    }

    public override void Fire1()
    {
        print(
            "Current heat factor: " + player.getHeatFactor()
            + " , damage: " + damage * (1f + (heatDamageRate * player.getHeatFactor()))
            + " , fire rate: " + bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        if (GetCooldown() <= 0 && numberOfProjectiles < maxProjectiles)
        {
            if (numberOfProjectiles == 0) {
                if (mousePosition.x > player.transform.position.x) {
                    spinDirection = -1;
                }
                else {
                    spinDirection = 1;
                }
            }
            float realDamage = damage * (1f + (heatDamageRate * player.getHeatFactor()));
            float realRadius = radius * (1f + (heatRadiusRate * player.getHeatFactor()));
            GameObject proj = Instantiate(
                projectile, 
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.5f), 
                Quaternion.Euler(0, 0, angle)
                );


            proj.GetComponent<BlueProjectile>().setDamage(realDamage);
            proj.GetComponent<BlueProjectile>().setRadius(realRadius);
            numberOfProjectiles++;
            proj.GetComponent<BlueProjectile>().id = numberOfProjectiles;

            SetCooldown(bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        }
    }

    public override void Fire2()
    {
    }
}
