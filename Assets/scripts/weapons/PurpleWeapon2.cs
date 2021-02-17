using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleWeapon2 : Weapon {

    public GameObject beam;
    //public GameObject beamExplosion;
    private Camera cam;
    private Vector2 mousePosition;
    private float angle;

    public float damage;
    private Rigidbody playerRB;
    private Light light;
    private ParticleSystem ps;
    private PlayerBehaviour player;

    public float heatDamageRate = 0.005f;
    public float heatRadiusRate = 0.004f;

    private AudioSource chargeSound;

    void Start () {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        light = GetComponent<Light>();
        ps = GetComponent<ParticleSystem>();
        ps.enableEmission = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        chargeSound = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (GetCooldown() > 0)
            DecrementCooldown();

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraFollow.CameraDistance));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        if (Input.GetButtonUp("Fire1"))
        {
            /*
            Ray ray = new Ray(transform.position, (Vector3)mousePosition - transform.position);
            RaycastHit[] targets = Physics.RaycastAll(ray, 50f);
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].collider.tag == "Enemy")
                {
                    targets[i].collider.GetComponent<Enemy>().getDamage(damage * chargeValue);
                    print("charge value: " + chargeValue + ", damage: " + damage * chargeValue);
                }
            }
            */
            GameObject newBeam = Instantiate(beam, transform.position, Quaternion.Euler(0, 0, angle));
            //Instantiate(beamExplosion, transform.position, Quaternion.Euler(-angle, 90, 0));

            light.intensity *= 15f;
            light.range *= 2f;
            ps.enableEmission = false;
            chargeSound.Stop();
        }
    }

    public override void Fire1()
    {
    }

    public override void Fire2()
    {
    }
}
