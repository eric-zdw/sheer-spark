using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedWeapon2 : Weapon {

    public GameObject projectile;
    private Camera cam;
    private PlayerBehaviour player;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.2f;

    public float heatDamageRate = 0.005f;
    public float heatFireRate = 0.004f;
    public float damage = 6f;
    public float shotSeparation = 1.5f;
    public float shotSpread = .1f;

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

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11f));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
    }

    public override void Fire1()
    {
        print(
            "Current heat factor: " + player.getHeatFactor()
            + " , damage: " + damage * (1f + (heatDamageRate * player.getHeatFactor()))
            + " , fire rate: " + bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        if (GetCooldown() <= 0)
        {
            
            /*
            for (int i = -1; i <= 1; i++)
            {
                float realDamage = damage * (1f + (heatDamageRate * player.getHeatFactor()));
                GameObject proj = Instantiate(
                    projectile, 
                    transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.25f), 
                    Quaternion.Euler(0, 0, angle + (shotSeparation * i) + Random.Range(-shotSpread * 0.5f, shotSpread * 0.5f))
                    );


                proj.GetComponent<RedProjectile>().setDamage(realDamage);
            }
            */
            
            for (int i = 0; i < 7; i++) {
                float realDamage = damage * (1f + (heatDamageRate * player.getHeatFactor()));
                GameObject proj = Instantiate(
                projectile,
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.25f),
                Quaternion.Euler(0, 0, angle + Random.Range(-shotSpread * 0.5f, shotSpread * 0.5f))
                );
                
                proj.GetComponent<RedProjectile2>().setDamage(realDamage);
            }
            
            
            SetCooldown(bFireRate / (1f + (heatFireRate * player.getHeatFactor())));
        }
    }

    public override void Fire2()
    {
    }
}
