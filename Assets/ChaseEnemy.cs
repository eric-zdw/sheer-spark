﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : Enemy {
	public GameObject healthBar;
	public float newHealth;
	private GameObject bar;

	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;


	private int jumpCounter;
	private bool isJumping = false;
	private float jumpTimer;
	public int jumpChance = 5;
	public float jumpStrength = 750f;

	public GameObject JumpParticleCharge;
	public GameObject JumpParticle;
	public GameObject[] powerups;
    public Material[] colours;
	public Material[] seeThroughs;
	public GameObject[] explosions;
    public float YLimit;
    private MeshRenderer outline;
	private MeshRenderer seeThrough;
    private int powerupRoll;

	private bool isCharging;
	private GameObject charge;

    private CameraFollow cam;

    public bool isTethered;
    private float tetheredCheck = 0.05f;
    private float tetheredTimer;

    void Start()
	{
		bar = Instantiate(healthBar);
		maxHealth = newHealth;
		health = maxHealth;
		bar.GetComponent<HealthBar>().setTarget(gameObject);

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();

        powerupRoll = Random.Range(0, 6);
        outline = transform.GetChild(0).GetComponent<MeshRenderer>();
		seeThrough = transform.GetChild(1).GetComponent<MeshRenderer>();
        outline.material = colours[powerupRoll];
		seeThrough.material = seeThroughs[powerupRoll];
    }

	void FixedUpdate()
	{
		if (health <= 0)
		{
			Explode();
		}
		
        if (player != null)
        {
            playerLocation = player.transform.position;

            if (transform.position.x < playerLocation.x && rb.velocity.x < 9f)
            {
                rb.AddForce(new Vector3(800f, 0f, 0f) * Time.deltaTime);
                rb.AddTorque(0, 0, -40f * Time.deltaTime);
            }
            else if (transform.position.x > playerLocation.x && rb.velocity.x > -9f)
            {
                rb.AddForce(new Vector3(-800f, 0f, 0f) * Time.deltaTime);
                rb.AddTorque(0, 0, 40f * Time.deltaTime);
            }
        }

		rb.AddForce(new Vector3(-rb.velocity.x, 0, 0) * 2f * Time.deltaTime);
		rb.AddForce(new Vector3(0, -rb.velocity.y, 0) * 100f * Time.deltaTime);


		//-------------------

		JumpRoutine();

        //-------------------
	    
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
            Explode();
        }
    }

    void Explode()
	{
        cam.addShake(0.5f);
		Instantiate(powerups[powerupRoll], transform.position, Quaternion.identity);
		Instantiate(explosions[powerupRoll], transform.position, transform.rotation);
		Destroy(bar);
		Destroy(gameObject);
	}

	void JumpRoutine()
	{
		if (jumpTimer > 0)
			jumpTimer -= Time.deltaTime;
		else
			isJumping = false;


		if (isJumping == false) 
		{
			jumpCounter = Random.Range(1, 1000);
			print("y: " + transform.position.y);
			if (jumpCounter <= jumpChance && transform.position.y <= YLimit)
			{
				print("jumped");
				charge = Instantiate(JumpParticleCharge, transform.position, Quaternion.identity);
				isJumping = true;
				isCharging = true;
				jumpTimer = 1f;
			}
		}
		if (isCharging)
			charge.transform.position = transform.position;
		if (jumpTimer <= 0.5f && isCharging) 
		{
			Instantiate(JumpParticle, transform.position, Quaternion.identity);
			isCharging = false;
			rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
			rb.AddForce(0f, jumpStrength, 0f);
		}
	}
}
