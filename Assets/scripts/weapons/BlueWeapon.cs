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

    public float maxHeatDamageMulti = 0.5f;
    public float maxHeatFireRateMulti = 1f;
    public float maxHeatRadiusMulti = 1f;
    public float damage = 6f;
    public float chargeDamage = 20f;

    public float rangeRadius = 0f;
    private float lastRR = 0f;
    private AudioSource[] sounds;

    public Material rangeMaterial;

    private BlueChargeHitbox chargeHit;

    public GameObject projectile;
    public float radius = 5f;

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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        numberOfProjectiles = 0;
    }

    void Update()
    {
        

        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraFollow.CameraDistance));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        if (Input.GetButtonUp("Fire1"))
        {
            BlueWeapon.numberOfProjectiles = 0;
        }

        //Debug.DrawLine(transform.position, transform.position + new Vector3(rangeRadius, 0, 0));
    }

    private void FixedUpdate() {
        if (GetCooldown() > 0)
            DecrementCooldown();
    }

    public override void Fire1()
    {
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
            float finalDamage = damage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(4)));
            float finalRadius = radius * (1f + (maxHeatRadiusMulti * player.GetHeatFactor(4)));
            GameObject proj = Instantiate(
                projectile, 
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.5f), 
                Quaternion.Euler(0, 0, angle)
                );


            proj.GetComponent<BlueProjectile>().setDamage(finalDamage);
            proj.GetComponent<BlueProjectile>().setRadius(finalRadius);
            numberOfProjectiles++;
            proj.GetComponent<BlueProjectile>().id = numberOfProjectiles;

            SetCooldown(bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        }
    }

    public override void Fire2()
    {
    }
}
