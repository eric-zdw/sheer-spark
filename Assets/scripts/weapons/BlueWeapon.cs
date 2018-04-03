using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueWeapon : Weapon {

    public GameObject projectile;
    private Camera cam;
    private PlayerBehaviour player;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.2f;

    public float heatDamageRate = 0.005f;
    public float heatFireRate = 0.004f;
    public float damage = 6f;

    public float rangeRadius = 0f;
    private float lastRR = 0f;
    private float rangeRate = 5f;
    private float maxRange = 6f;

    public GameObject rangeSphere;
    private GameObject rs;
    private AudioSource[] sounds;

    void Start () {
        SetFireRate(bFireRate);
        WeaponType weaponType = WeaponType.Automatic;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        rs = Instantiate(rangeSphere, transform.position, Quaternion.identity, transform);
        sounds = GetComponents<AudioSource>();
    }

    void Update()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11f));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        rs.transform.position = transform.position;
        rs.transform.localScale = new Vector3(rangeRadius * (2f/0.3f), rangeRadius * (2f / 0.3f), rangeRadius * 0.1f);
        if (Input.GetButton("Fire1"))
        {
            rangeRadius += rangeRate * (maxRange - rangeRadius) * Time.deltaTime;
            lastRR = rangeRadius;
        }
        else
            rangeRadius = 0f;

        if (Input.GetButtonUp("Fire1"))
        {
            sounds[0].volume = (lastRR / maxRange) * 0.5f;
            sounds[0].Play();
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
            GameObject proj = Instantiate(
                projectile, 
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.5f), 
                Quaternion.Euler(0, 0, angle)
                );


            proj.GetComponent<BlueProjectile>().setDamage(realDamage);

            SetCooldown(bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        }
    }

    public override void Fire2()
    {
    }
}
