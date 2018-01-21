using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleWeapon : Weapon {

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
            Ray ray = new Ray(transform.position, (Vector3)mousePosition - transform.position);
            RaycastHit[] targets = Physics.RaycastAll(ray, 50f);
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].collider.tag == "Enemy")
                {
                    targets[i].collider.GetComponent<Enemy>().getDamage(100f);
                }
            }
            Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle));
            print("purple shooting");
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
