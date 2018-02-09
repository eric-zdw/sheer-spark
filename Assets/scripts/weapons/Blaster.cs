using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : Weapon {

    public GameObject projectile;
    public GameObject projectile2;
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
            Instantiate(projectile, transform.position + (Vector3.Normalize((Vector3)mousePosition - transform.position) * 0.2f), Quaternion.Euler(0, 0, angle + Random.Range(-2f, 2f)));
            SetCooldown(bFireRate);
        }
    }

    public override void Fire2()
    {
        if (GetCooldown() <= 0)
        {
            Instantiate(projectile2, transform.position, Quaternion.Euler(0, 0, angle));
            SetCooldown(secondaryRate);
        }
    }
}
