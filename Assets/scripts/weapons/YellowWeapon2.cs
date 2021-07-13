using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowWeapon2 : Weapon {

    public GameObject projectile;
    public GameObject superProjectile;
    private Camera cam;
    private PlayerBehaviour player;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.2f;
    public float superFireRate = 0.8f;
    public float altSpread = 5f;

    public float maxHeatDamageMulti = 0.5f;
    public float maxHeatFireRateMulti = 1f;
    public float maxHeatRadiusMulti = 1f;
    public float damage = 6f;
    public float superDamage = 80f;
    public float radius = 1f;
    public float superRadius = 6f;
    public bool debugMode = false;

    int altFire = 1;

    void Start () {
        SetFireRate(bFireRate);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void FixedUpdate()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
    }

    public override void Fire1()
    {
        /*print(
            "Current heat factor: " + player.getHeatFactor()
            + " , damage: " + damage * (1f + (heatDamageRate * player.getHeatFactor()))
            + " , fire rate: " + bFireRate / (1f + (heatFireRate * player.getHeatFactor())));*/
        if (GetCooldown() <= 0)
        {
            altFire *= -1;
            float realDamage = damage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(EnergyColor.Yellow)));
            float realRadius = radius * (1f + (maxHeatRadiusMulti * player.GetHeatFactor(EnergyColor.Yellow)));
            GameObject proj = Instantiate(
                projectile,
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.2f),
                Quaternion.Euler(0, 0, angle + Random.Range(-5f, 5f) + (altFire * 5f))
                );
            proj.GetComponent<YellowProjectile2>().setDamage(realDamage);
            proj.GetComponent<YellowProjectile2>().setRadius(realRadius);
            SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Yellow))));
        }
    }

    public override void Fire2()
    {
        if (GetCooldown() <= 0 && (player.powerupEnergy[2] >= energyCost || debugMode))
        {
            float realDamage = superDamage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(EnergyColor.Yellow)));
            float realRadius = superRadius * (1f + (maxHeatRadiusMulti * player.GetHeatFactor(EnergyColor.Yellow)));
            for (int i = -1; i <= 1; i++)
            {
                GameObject proj = Instantiate(
                    superProjectile,
                    transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.2f),
                    Quaternion.Euler(0, 0, angle + (i * altSpread))
                    );
                proj.GetComponent<YellowProjectile>().setDamage(realDamage);
                proj.GetComponent<YellowProjectile>().setRadius(realRadius);
            }

            SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Yellow))));

            if (!debugMode)
            {
                player.powerupEnergy[2] -= energyCost;
                player.energyPanel.UpdateEnergyMeters();
            }

            SetCooldown(superFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Yellow))));
        }
    }
}
