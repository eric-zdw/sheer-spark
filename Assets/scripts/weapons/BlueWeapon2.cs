using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueWeapon2 : Weapon {

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

    public float rangeRadius = 0f;
    private float lastRR = 0f;
    private float rangeRate = 2f;
    private float maxRange = 6f;

    public GameObject rangeSphere;
    private GameObject rs;
    private AudioSource[] sounds;

    void Start () {
        SetFireRate(bFireRate);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        rs = Instantiate(rangeSphere, transform.position, Quaternion.identity, transform);
        sounds = GetComponents<AudioSource>();
    }

    void FixedUpdate()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11f));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        //Debug.DrawLine(transform.position, transform.position + new Vector3(rangeRadius, 0, 0));
    }

    public override void Fire1()
    {
        if (GetCooldown() <= 0)
        {
            float realDamage = damage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(EnergyColor.Blue)));
            GameObject proj = Instantiate(
                projectile, 
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.5f), 
                Quaternion.Euler(0f, 0f, angle + 90f)
                );


            proj.GetComponent<BlueProjectile2>().setDamage(realDamage);

            SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Blue))));
        }
    }

    public override void Fire2()
    {
    }
}
