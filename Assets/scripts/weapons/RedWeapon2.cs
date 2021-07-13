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
    public float maxHeatDamageMulti = 0.5f;
    public float maxHeatFireRateMulti = 1f;
    public float maxHeatRadiusMulti = 1f;
    public float damage = 6f;
    public float shotSeparation = 1.25f;
    public float shotSpread = 0f;

    void Start () {
        SetFireRate(bFireRate);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void FixedUpdate()
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
                Quaternion.Euler(0, 0, angle + Random.Range(-shotSpread * 0.5f, shotSpread * 0.5f))
                );


            proj.GetComponent<RedProjectile1>().setDamage(realDamage);
            /*
            for (int i = -8; i <= 8; i++)
            {
                float realDamage = damage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(EnergyColor.Red)));
                GameObject proj = Instantiate(
                    projectile, 
                    transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.25f), 
                    Quaternion.Euler(0, 0, angle + (shotSeparation * i) + Random.Range(-shotSpread * 0.5f, shotSpread * 0.5f))
                    );


                proj.GetComponent<RedProjectile1>().setDamage(realDamage);
            }
            */

            /*
            float realDamage = damage * (1f + (heatDamageRate * player.getHeatFactor()));
            GameObject proj = Instantiate(
                projectile,
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.25f),
                Quaternion.Euler(0, 0, angle + Random.Range(-shotSpread * 0.5f, shotSpread * 0.5f))
                );
                
            proj.GetComponent<RedProjectile>().setDamage(realDamage);
            */

            SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Red))));
        }
    }

    public override void Fire2()
    {
    }
}
