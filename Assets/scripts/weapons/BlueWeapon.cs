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
    private float rangeRate = 2f;
    public float maxRange = 6f;

    public GameObject rangeSphere;
    private GameObject rs;
    private AudioSource[] sounds;

    public Material rangeMaterial;
    private NoiseManager noiseManager;

    private BlueChargeHitbox chargeHit;

    public GameObject projectile;
    public float radius = 5f;
    public float heatRadiusRate = 0.002f;

    void Start () {
        SetFireRate(bFireRate);
        WeaponType weaponType = WeaponType.Automatic;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        rs = Instantiate(rangeSphere, transform.position, Quaternion.identity, transform);
        noiseManager = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<NoiseManager>();
    }

    void Update()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11f));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        rs.transform.position = transform.position;
        rs.transform.localScale = new Vector3(rangeRadius * (2f/0.3f), rangeRadius * (2f / 0.3f), rangeRadius * (2f / 0.3f));
        /*
        if (Input.GetButton("Fire1"))
        {
            rangeRadius += rangeRate * (maxRange - rangeRadius) * Time.deltaTime;
            lastRR = rangeRadius;
            float r = 0.25f * (rangeRadius / maxRange);
            float g = 0.375f * (rangeRadius / maxRange);
            float b = 0.5f * (rangeRadius / maxRange);
            float a = 0.5f * (rangeRadius / maxRange);
            rangeMaterial.SetColor("_TintColor", new Color(r, g, b, a));
        }  
        */ 

        if (Input.GetButtonUp("Fire1"))
        {
            //sounds[0].volume = (lastRR / maxRange) * 0.5f;
            //sounds[0].Play();
            //GameObject.Destroy(chargeHit.gameObject);

            //rangeMaterial.SetColor("_TintColor", new Color(0f, 0f, 0f, 0f));

            /*
            noiseManager.AddNoise(25f * rangeRadius);
            GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity);
            exp.transform.localScale = new Vector3(rangeRadius * 2f, rangeRadius * 2f, rangeRadius * 2f);

            BlueExplosionHitbox expHit = Instantiate(explosionHitbox, transform.position, Quaternion.identity).GetComponent<BlueExplosionHitbox>();
            expHit.GetComponent<SphereCollider>().radius = rangeRadius * 6f;
            expHit.setDamage(damage * (rangeRadius / maxRange));
            expHit.setRadius(rangeRadius * 6f);
            */

            //rangeRadius = 0f;
            //lastRR = rangeRadius;
        }

        //Debug.DrawLine(transform.position, transform.position + new Vector3(rangeRadius, 0, 0));
    }

    public override void Fire1()
    {
        print(
            "Current heat factor: " + player.getHeatFactor()
            + " , damage: " + damage * (1f + (heatDamageRate * player.getHeatFactor()))
            + " , fire rate: " + bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        if (GetCooldown() <= 0)
        {
            float realDamage = damage * (1f + (heatDamageRate * player.getHeatFactor()));
            float realRadius = radius * (1f + (heatRadiusRate * player.getHeatFactor()));
            GameObject proj = Instantiate(
                projectile, 
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.5f), 
                Quaternion.Euler(0, 0, angle)
                );


            proj.GetComponent<BlueProjectile>().setDamage(realDamage);
            proj.GetComponent<BlueProjectile>().setRadius(realRadius);

            SetCooldown(bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        }
    }

    public override void Fire2()
    {
    }
}
