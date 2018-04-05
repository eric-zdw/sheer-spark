using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleWeapon : Weapon {

    public GameObject beam;
    //public GameObject beamExplosion;
    private Camera cam;
    private Vector2 mousePosition;
    private float angle;
    public float bFireRate = 0.16f;
    public float secondaryRate = 0.90f;

    public float damage;
    public float maxSize;
    private float chargeValue = 0f;
    public float chargeRate;
    public float recoilForce;
    private Rigidbody playerRB;
    private Light light;
    private ParticleSystem ps;
    private PlayerBehaviour player;

    public float heatDamageRate = 0.005f;
    public float heatRadiusRate = 0.004f;

    private AudioSource chargeSound;

    void Start () {
        SetFireRate(bFireRate);
        WeaponType weaponType = WeaponType.Automatic;
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

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        if (Input.GetButtonUp("Fire1") && chargeValue > 0.15f)
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
            newBeam.GetComponent<ParticleSystem>().startSize = (maxSize * chargeValue * (1f + (heatRadiusRate * player.getHeatFactor())));
            newBeam.GetComponent<CapsuleCollider>().radius = maxSize * (0.5f) * chargeValue * (1f + (heatRadiusRate * player.getHeatFactor()));
            newBeam.GetComponent<PurpleBeam>().setDamage(damage * chargeValue * (1f + (heatDamageRate * player.getHeatFactor())));


            float trueRecoil = recoilForce * chargeValue;
            playerRB.AddForce(new Vector3(-trueRecoil * Mathf.Cos(angle * Mathf.Deg2Rad), -trueRecoil * Mathf.Sin(angle * Mathf.Deg2Rad), 0));

            chargeValue = 0f;
            light.intensity *= 15f;
            light.range *= 2f;
            ps.enableEmission = false;
            chargeSound.Stop();
        }
        else if (Input.GetButtonUp("Fire1") && chargeValue < 0.15f)
        {
            chargeValue = 0f;
            ps.enableEmission = false;
            chargeSound.Stop();
        }

        if (chargeValue == 0f && light.intensity >= 0)
        {
            light.intensity *= 0.75f;
            light.range *= 0.9f;
        }

        if (chargeValue > 0f)
        {
            chargeSound.volume = (chargeValue * 0.25f) + 0.25f;
            chargeSound.pitch = (chargeValue * 0.5f) + 0.5f;
        }
        else
        {
            chargeSound.volume = 0f;
            chargeSound.pitch = 0f;
        }
            

    }

    public override void Fire1()
    {
        print(
        "Current heat factor: " + player.getHeatFactor()
        + " , damage: " + damage * (1f + (heatDamageRate * player.getHeatFactor()))
        + " , maxSize: " + maxSize * (1f + (heatRadiusRate * player.getHeatFactor())));

        ps.enableEmission = true;
        if (chargeSound.isPlaying == false)
            chargeSound.Play();

        if (chargeValue < 1f)
        {
            chargeValue += chargeRate * Time.deltaTime;
            if (chargeValue > 1f)
                chargeValue = 1f;
        }
        light.intensity = chargeValue * 3f;
        light.range = chargeValue * 25f;
    }

    public override void Fire2()
    {
    }
}
