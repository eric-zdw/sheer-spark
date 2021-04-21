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
    public float maxHeatDamageMulti = 0.5f;
    public float maxHeatFireRateMulti = 1f;
    public float maxHeatRadiusMulti = 1f;
    public float damage = 6f;
    public float healthPercentageAsDamage = 0.2f;
    public float radius = 1f;

    private bool projectileOut = false;
    public bool holdingEnemy = false;
    public GameObject heldEnemy;
    private Vector3 heldVelocity;

    public GameObject collisionDetectPrefab;

    //  Damage is based on three components:
    //  1. Raw damagee
    //  2. Max health of enemy (percentage)
    //  (3.) Speed of collision?
    //  No effect on bosses (maybe the raw damage will proc). Enemies will spawn that can be thrown into bosses.

    void Start() {
        SetFireRate(bFireRate);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    private void Update()
    {
        if (holdingEnemy && Input.GetMouseButtonDown(0))
        {
            Vector3 shootDirection = transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 5000f);
            float mass = heldEnemy.GetComponent<Rigidbody>().mass;
            heldEnemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
            heldEnemy.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(shootDirection - heldEnemy.transform.position) * 2250f * (0.5f + mass * 0.4f));

            float initialDamage = damage + (heldEnemy.GetComponent<Enemy>().getMaxHealth() * healthPercentageAsDamage);
            float realDamage = initialDamage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(EnergyColor.Green)));
            GameObject collisionDetect = Instantiate(collisionDetectPrefab, heldEnemy.transform.position, Quaternion.identity, heldEnemy.transform);
            collisionDetect.GetComponent<GreenWeapon2Collision>().damage = realDamage;

            holdingEnemy = false;
            heldEnemy = null;
            //throw enemy
            SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Green))));
        }
    }

    private void FixedUpdate()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraFollow.CameraDistance));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        if (holdingEnemy)
        {
            Vector3 holdPosition = transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 5f);
            heldEnemy.GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(heldEnemy.GetComponent<Rigidbody>().velocity, Vector3.zero, ref heldVelocity, 0.2f);
            heldEnemy.GetComponent<Rigidbody>().AddForce((holdPosition - heldEnemy.transform.position) * 2250f * (0.5f + heldEnemy.GetComponent<Rigidbody>().mass * 0.5f) * Time.deltaTime);
            //heldEnemy.GetComponent<Rigidbody>().AddExplosionForce(-15000f * Time.deltaTime, holdPosition, 100f, 0f);
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
            float realDamage = damage * (1f + (maxHeatDamageMulti * player.GetHeatFactor(EnergyColor.Green)));
            float realRadius = radius * (1f + (maxHeatRadiusMulti * player.GetHeatFactor(EnergyColor.Green)));
            GameObject proj = Instantiate(
                projectile,
                transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.8f),
                Quaternion.Euler(0, 0, angle)
                );
            proj.GetComponent<GreenProjectile2>().setDamage(realDamage);
            SetCooldown(bFireRate / (1f + (maxHeatFireRateMulti * player.GetHeatFactor(EnergyColor.Green))));
        }
    }

    public override void Fire2()
    {

    }
}
