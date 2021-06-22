using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleWeapon2 : Weapon
{

    public GameObject projectile;
    //public GameObject beamExplosion;
    private Camera cam;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.16f;
    public float secondaryRate = 0.90f;

    public float damage;
    public float maxSize;
    private float chargeValue = 0f;
    public float chargeRate;
    public float recoilForce;
    private Rigidbody playerRB;
    private Light light;
    private ParticleSystem ps;
    private PlayerBehaviour player;

    public float maxHeatDamageMulti = 0.5f;
    public float maxHeatFireRateMulti = 1f;
    public float maxHeatRadiusMulti = 1f;

    private AudioSource chargeSound;

    void Start()
    {
        SetFireRate(bFireRate);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        light = GetComponent<Light>();
        ps = GetComponent<ParticleSystem>();
        ps.enableEmission = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        chargeSound = GetComponent<AudioSource>();
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

            float realDamage = damage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(EnergyColor.Red)));
            GameObject proj = Instantiate(
            projectile,
            transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.25f),
            Quaternion.Euler(0, 0, angle)
            );

            proj.GetComponent<PurpleArrow>().setDamage(realDamage);

            SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Red))));
            print("cooldown: " + player.GetHeatFactor(EnergyColor.Red));
        }
    }

    public override void Fire2()
    {
    }
}
