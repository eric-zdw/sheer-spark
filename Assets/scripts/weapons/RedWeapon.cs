using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedWeapon : Weapon {

    public GameObject projectile;
    private Camera cam;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.16f;
    public float secondaryRate = 0.90f;

    void Start () {
        SetFireRate(bFireRate);
        WeaponType weaponType = WeaponType.Automatic;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
    }

    public override void Fire1()
    {
        if (GetCooldown() <= 0)
        {
            float randomFactor = Random.Range(-5f, 5f);
            for (int i = -5; i <= 5; i++)
            {
                Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle + (2f * i) + Random.Range(-1f, 1f) + randomFactor));
            }
            SetCooldown(bFireRate);
        }
    }

    public override void Fire2()
    {
    }
}
