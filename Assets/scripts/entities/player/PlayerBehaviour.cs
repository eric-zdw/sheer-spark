﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    public float playerBaseSpeed = 25f;
    public float playerHeatSpeed = 25f;
    public float verticalSpeed;
    private float playerSpeed;

    public float dragForceX = 0.025f;
    public float dragForceY = 0.01f;
    public float maxAngularVelocity = 15f;
    public float torqueStrength = 0.5f;
    // public float jumpStrength = 5f;
    public float maxHP = 3;
    private Rigidbody rb;

    private GameObject weaponSlot;
    private Weapon weapon;
    private Utility util;
    private GameObject utilitySlot;
    private GameObject itemSlot1;
    private GameObject itemSlot2;

    public int[] sparkEnergy;

    private MeshRenderer mesh;
    public Material defaultColour;

    public GameObject defaultWeapon;
    public float powerupDuration;
    public float powerupIncrement;
    public float powerupTimer;
    public float powerupHealth;

    public float powerupLevel;
    public bool hasPowerup;
    private float heatTimer = 0f;
    public float heatFactor;
    public float heatDecayFactor;
    public float heatConstantDecay;
    public float HP;
    public EnergyPanel energyPanel;

    private AudioSource collisionSound;

    public GameObject deathExplosion;
    public Color[] powerColors;
    private GameObject[] powerups;
    private RectTransform pbSize;

    public GameObject playerUI;
    private PowerupRadial radialBar;
    private PlayerHealthBar healthBar;
    public ParticleSystemRenderer lightTrail;
    public Material[] trailMaterials;

    private MaterialPropertyBlock matBlock;
    public GameObject[] weaponsList;

    private DamageAlert damageAlert;

    private GameObject inside;
    private GameObject inside2;
    private GameObject inside3;

    public float invincible = 0f;
    public bool isDashing = false;

    public AudioSource damageClip;

    public bool localGravity = false;

    public static int projectileLayerMask = ~((1 << 9) | (1 << 13) | (1 << 8) | (1 << 14) | (1 << 18) | (1 << 21));

    // Use this for initialization
    void Start()
    {
        playerSpeed = playerBaseSpeed;

        rb = GetComponent<Rigidbody>();
        weaponSlot = GameObject.Find("WeaponSlot");
        utilitySlot = GameObject.Find("UtilitySlot");
        itemSlot1 = GameObject.Find("ItemSlot1");
        itemSlot2 = GameObject.Find("ItemSlot2");

        weapon = weaponSlot.GetComponentInChildren<Weapon>();
        util = utilitySlot.GetComponentInChildren<Utility>();
        
        sparkEnergy = new int[6];

        mesh = GetComponent<MeshRenderer>();

        rb.maxAngularVelocity = maxAngularVelocity;
        HP = maxHP;

        collisionSound = GetComponent<AudioSource>();

        radialBar = playerUI.GetComponentInChildren<PowerupRadial>();
        healthBar = playerUI.GetComponentInChildren<PlayerHealthBar>();
        matBlock = new MaterialPropertyBlock();
        damageAlert = GameObject.Find("DamageAlert").GetComponent<DamageAlert>();

        powerups = new GameObject[6];

        inside = GameObject.Find("Inside");
        inside2 = GameObject.Find("Inside2");
        inside3 = GameObject.Find("Inside3");

        //weapon selection
        transform.position = GameObject.Find("PlayerSpawn").transform.position;

        if (localGravity) {
            rb.useGravity = false;
        }
    }

    void Update()
    {
        Fire();

        if (powerupTimer >= 0f)
        {
            powerupTimer -= Time.deltaTime;
            if (pbSize != null)
            {
                pbSize.localScale = new Vector3(powerupTimer / powerupDuration, 1f, 1f);
            }

            if (powerupTimer <= 0f)
            {
                hasPowerup = false;
                Destroy(weapon.gameObject);
                weapon = Instantiate(defaultWeapon, transform.position, Quaternion.identity, weaponSlot.transform).GetComponent<Weapon>();
                mesh.material = defaultColour;
                inside.GetComponent<MeshRenderer>().material = defaultColour;
                inside2.GetComponent<MeshRenderer>().material = defaultColour;
                inside3.GetComponent<MeshRenderer>().material = defaultColour;
                GameObject.FindGameObjectWithTag("PlayerLight").GetComponent<Light>().color = new Color(0.549f, 0.608f, 0.678f);
                //powerupBar.GetComponent<UnityEngine.UI.Image>().material = defaultColour;
                //powerupName.text = "BLASTER";
                lightTrail.material = trailMaterials[0];
            }
        }

        if (powerupLevel >= 7.5f)
        {
            heatFactor = 1f;
            playerSpeed = playerBaseSpeed + (playerHeatSpeed * heatFactor);
        }

        if (powerupLevel <= 0f)
        {
            playerSpeed = playerBaseSpeed;
        }
        else if (powerupLevel > 0f)
        {
            powerupLevel -= Time.deltaTime * 0.6f;
        }

        if (invincible > 0f) {
            invincible -= Time.deltaTime;
        }
    }

    void FixedUpdate () {
        Movement();

        if (!isDashing) {
            rb.AddForce(new Vector3(-rb.velocity.x, 0, 0) * dragForceX * Time.deltaTime);
            rb.AddForce(new Vector3(0, -rb.velocity.y, 0) * dragForceY * Time.deltaTime);
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0f, 0f);
        if (horizontal < -0.5f && rb.velocity.x > -4f)
            rb.AddForce(movement * playerSpeed * 1.5f * Time.deltaTime);
        else if (horizontal > 0.5f && rb.velocity.x < 4f)
            rb.AddForce(movement * playerSpeed * 1.5f * Time.deltaTime);
        else
            rb.AddForce(movement * playerSpeed * Time.deltaTime);
        rb.AddTorque(0, 0, -horizontal * torqueStrength * Time.deltaTime);

        float vertical = Input.GetAxis("Vertical");
        Vector3 verticalMovement = new Vector3(0f, vertical, 0f);
        if (vertical < 0f)
            rb.AddForce(verticalMovement * verticalSpeed * Time.deltaTime);
    }

    void Fire()
    {
        if (Input.GetButton("Fire1"))
        {
            weapon.Fire1();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            weapon.Fire2();
        }
        if (Input.GetButtonDown("Fire3"))
        {
            util.Activate();
        }
    }

    public void getPowerup(GameObject newWeapon, Material newColour, int index, Material newMaterial, string powerName)
    {
        hasPowerup = true;
        ScoreManager.IncreaseScore(500);
        ScoreManager.IncreaseMultiplier(1f);
        weapon = newWeapon.GetComponent<Weapon>();
        mesh.material = newColour;
        inside.GetComponent<MeshRenderer>().material = newColour;
        inside2.GetComponent<MeshRenderer>().material = newColour;
        inside3.GetComponent<MeshRenderer>().material = newColour;

        powerupTimer = Mathf.Clamp(powerupTimer + powerupIncrement, 0f, powerupDuration);
        GameObject.FindGameObjectWithTag("PlayerLight").GetComponent<Light>().color = powerColors[index];

        sparkEnergy[index] = Mathf.Clamp(sparkEnergy[index] + 1, 0, 6);
        energyPanel.UpdateEnergyMeters();

        radialBar.changePowerup(newColour);
        //offset for default trail color
        lightTrail.material = trailMaterials[index + 1];

        if (HP < maxHP) {
            StartCoroutine(healthBar.Flash());
        }

        HP = Mathf.Clamp(HP + powerupHealth, 0f, maxHP);
    }

    public void addRecoil(Vector3 direction)
    {
        rb.AddForce(direction);
    }

    public float getHeatFactor()
    {
        return heatFactor;
    }

    public void takeDamage(int damage)
    {
        //death condition: player has no health and takes more damage.
        damageClip.Play();

        if (damage > 0f && HP == 0f) {
            Camera.main.GetComponent<CameraFollow>().AddNoise(80f);
            Instantiate(deathExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else {
            HP = Mathf.Max(0f, HP - damage);
		    Camera.main.GetComponent<CameraFollow>().AddNoise(25f);
        }

        invincible = 2f;

        StartCoroutine(healthBar.Flash());
        damageAlert.Flash(new Color(1f, 0f, 0f));
    }

    public void OnCollisionEnter(Collision collision)
    {
        collisionSound.volume = collision.relativeVelocity.magnitude * 0.05f;
        collisionSound.Play();
    }

}