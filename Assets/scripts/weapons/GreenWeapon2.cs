using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenWeapon2 : Weapon {

    public GameObject projectile;
    private Camera cam;
    private PlayerBehaviour player;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.2f;

    public float heatDamageRate = 0.005f;
    public float heatFireRate = 0.004f;
    public float heatRadiusRate = 0.002f;
    public float damage = 6f;
    public float radius = 1f;

    private bool projectileOut = false;
    public bool holdingEnemy = false;
    public GameObject heldEnemy;
    private Vector3 heldVelocity;

    void Start () {
        SetFireRate(bFireRate);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    private void FixedUpdate()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraFollow.CameraDistance));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        if (holdingEnemy)
        {
            Vector3 holdPosition = transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 10f);
            heldEnemy.GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(heldEnemy.GetComponent<Rigidbody>().velocity, Vector3.zero, ref heldVelocity, 0.2f);
            heldEnemy.GetComponent<Rigidbody>().AddExplosionForce(-15000f * Time.deltaTime, holdPosition, 100f, 0f);
        }
    }

    public override void Fire1()
    {
        /*print(
            "Current heat factor: " + player.getHeatFactor()
            + " , damage: " + damage * (1f + (heatDamageRate * player.getHeatFactor()))
            + " , fire rate: " + bFireRate / (1f + (heatFireRate * player.getHeatFactor())));*/
        if (GetCooldown() <= 0 && !projectileOut && !holdingEnemy)
        {
            float realDamage = damage * (1f + (heatDamageRate * player.GetHeatFactor(EnergyColor.Green)));
            float realRadius = radius * (1f + (heatRadiusRate * player.GetHeatFactor(EnergyColor.Green)));
            GameObject proj = Instantiate(
                projectile,
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.8f),
                Quaternion.Euler(0, 0, angle)
                );
            proj.GetComponent<GreenProjectile2>().setDamage(realDamage);
            SetCooldown(bFireRate / (1f + (heatFireRate * player.GetHeatFactor(EnergyColor.Green))));
        }
    }

    public override void Fire2()
    {

    }

    public void OnMouseDown()
    {
        if (holdingEnemy)
        {
            //throw enemy
        }
    }
}
