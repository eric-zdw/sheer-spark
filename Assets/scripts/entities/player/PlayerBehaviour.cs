﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    public float playerSpeed = 20f;
    public float dragForceX = 0.025f;
    public float dragForceY = 0.01f;
    public float maxAngularVelocity = 15f;
    public float torqueStrength = 0.5f;
    public float jumpStrength = 5f;
    public int maxHP = 3;
    private Rigidbody rb;

    private GameObject weaponSlot;
    private Weapon weapon;
    private Utility util;
    private GameObject utilitySlot;
    private GameObject itemSlot1;
    private GameObject itemSlot2;

    private MeshRenderer mesh;
    public Material defaultColour;

    public GameObject defaultWeapon;
    public float powerupDuration;
    private float powerupTimer;

    public float heatFactor;
    public float heatDecayFactor;
    public float heatConstantDecay;
    private float decayTimer;
    private int HP;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        weaponSlot = GameObject.Find("WeaponSlot");
        utilitySlot = GameObject.Find("UtilitySlot");
        itemSlot1 = GameObject.Find("ItemSlot1");
        itemSlot2 = GameObject.Find("ItemSlot2");

        weapon = weaponSlot.GetComponentInChildren<Weapon>();
        util = utilitySlot.GetComponentInChildren<Utility>();

        mesh = GetComponent<MeshRenderer>();

        rb.maxAngularVelocity = maxAngularVelocity;
        decayTimer = 0.1f;
        HP = maxHP;
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Debug.Break();
        }

        if (powerupTimer >= 0f)
        {
            powerupTimer -= Time.deltaTime;
            if (powerupTimer <= 0f)
            {
                Destroy(weapon.gameObject);
                weapon = Instantiate(defaultWeapon, transform.position, Quaternion.identity, weaponSlot.transform).GetComponent<Weapon>();
                mesh.material = defaultColour;
            }
        }

        decayTimer -= Time.deltaTime;
        if (decayTimer <= 0f)
        {
            if (heatFactor > 0)
            {
                heatFactor *= heatDecayFactor;
                heatFactor -= heatConstantDecay;
            }
            else
                heatFactor = 0f;

            decayTimer += 0.1f;
        }
    }

    void FixedUpdate () {
        Movement();
        Jump();
        Fire();

        rb.AddForce(new Vector3(-rb.velocity.x, 0, 0) * dragForceX * Time.deltaTime);
        rb.AddForce(new Vector3(0, -rb.velocity.y, 0) * dragForceY * Time.deltaTime);
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
        
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(0f, jumpStrength, 0f);
        }
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

    public void getPowerup(GameObject newWeapon, Material newColour)
    {
        weapon = newWeapon.GetComponent<Weapon>();
        mesh.material = newColour;
        powerupTimer = powerupDuration;
        heatFactor += 1f;
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
        HP -= damage;
    }

}
