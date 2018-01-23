using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    public float playerSpeed = 20f;
    public float dragForceX = 0.025f;
    public float dragForceY = 0.01f;
    public float maxAngularVelocity = 15f;
    public float torqueStrength = 0.5f;
    public float jumpStrength = 5f;
    private Rigidbody rb;

    private GameObject weaponSlot;
    private Weapon weapon;
    private Utility util;
    private GameObject utilitySlot;
    private GameObject itemSlot1;
    private GameObject itemSlot2;

    private MeshRenderer mesh;
    public Material defaultColour;


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
        if (Input.GetButton("Fire2"))
        {
            weapon.Fire2();
        }
        if (Input.GetButtonDown("Fire3"))
        {
            util.Activate();
        }
    }

    public void RelinkWeapon(GameObject newWeapon)
    {
        weapon = newWeapon.GetComponent<Weapon>();
    }

    public void ChangeColour(Material newColour)
    {
        mesh.material = newColour;
    }

    public void addRecoil(Vector3 direction)
    {
        rb.AddForce(direction);
    }

}
