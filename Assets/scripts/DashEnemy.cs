﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : Enemy {
	public GameObject healthBar;
	public float newHealth;
	private GameObject bar;

	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;
	public GameObject[] powerups;
    public Material[] colours;
	public Material[] seeThroughs;
	public GameObject[] explosions;
	public Material damagedMaterial;
    public float YLimit;
	private MeshRenderer damageFlash;
    private MeshRenderer outline;
	private MeshRenderer seeThrough;
    private int powerupRoll;

	private bool isCharging;
	private GameObject charge;

    public bool isTethered;
    private float tetheredCheck = 0.05f;
    private float tetheredTimer;

	private PPManager ppManager;

	private MaterialPropertyBlock damageMatBlock;

	public float moveSpeed;

	private bool isDashing = false;

	private int layermask = ~(1 << 9 | 1 << 13 | 1 << 8 | 1 << 14);

	private float dashDistance = 75f;

    void Start()
	{
		bar = Instantiate(healthBar);
		maxHealth = newHealth * WaveSystem.enemyPower;
		health = maxHealth;
		bar.GetComponent<HealthBar>().setTarget(gameObject);

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();

		powerupRoll = Random.Range(0, 6);
		damageFlash = transform.GetChild(2).GetComponent<MeshRenderer>();
        outline = transform.GetChild(0).GetComponent<MeshRenderer>();
		seeThrough = transform.GetChild(1).GetComponent<MeshRenderer>();
        outline.material = colours[powerupRoll];
		seeThrough.material = seeThroughs[powerupRoll];

		for (int i = 3; i <= 6; i++)
		{
			outline = transform.GetChild(i).GetComponent<MeshRenderer>();
			outline.material = colours[powerupRoll];
		}
		for (int i = 7; i <= 10; i++)
		{
			seeThrough = transform.GetChild(i).GetComponent<MeshRenderer>();
			seeThrough.material = seeThroughs[powerupRoll];
		}

		damageMatBlock = new MaterialPropertyBlock();

		GetComponent<Rigidbody>().maxAngularVelocity = 40f;
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

			float rand = Random.Range(0f, 1f);

			if (!isDashing) {
            	if (transform.position.x < playerLocation.x)
            	{
            	    rb.AddForce(new Vector3(moveSpeed, 0f, 0f) * Time.deltaTime);
            	    rb.AddTorque(0, 0, -moveSpeed * 0.2f * Time.deltaTime);
					
					if (rand <= 0.012f && !isDashing && Vector3.Distance(transform.position, player.transform.position) < dashDistance) {
						StartCoroutine(DashRoutine(1));
					}
            	}
            	else if (transform.position.x > playerLocation.x)
            	{
            	    rb.AddForce(new Vector3(-moveSpeed, 0f, 0f) * Time.deltaTime);
            	    rb.AddTorque(0, 0, moveSpeed * 0.2f * Time.deltaTime);

					if (rand <= 0.012f && !isDashing && Vector3.Distance(transform.position, player.transform.position) < dashDistance) {
						StartCoroutine(DashRoutine(-1));
					}
            	}
			}
        }

		
		rb.AddForce(new Vector3(-rb.velocity.x, 0, 0) * 2f * Time.deltaTime);
		rb.AddForce(new Vector3(0, -rb.velocity.y, 0) * 80f * Time.deltaTime);
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
			if (collision.gameObject.GetComponent<PlayerBehaviour>().invincible <= 0f) {
				collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
            	getDamage(100);
			}
        }
    }

    void Explode()
	{
		Camera.main.GetComponent<CameraFollow>().AddNoise(10f);
		Instantiate(powerups[powerupRoll], transform.position, Quaternion.identity);
		Instantiate(explosions[powerupRoll], transform.position, transform.rotation);
		Destroy(bar);
		Destroy(gameObject);
	}

	public override void getDamage(float damage)
    {
        health -= damage;
		StartCoroutine(FlashWhite());
    }

	IEnumerator FlashWhite() {
		float colorValue = 2f;
		Color newColor = new Color(colorValue, colorValue, colorValue, 1);
		while (colorValue > 0f) {
			//print("colorValue: " + colorValue);
			colorValue -= 5f * Time.deltaTime;
			newColor = new Color(colorValue, colorValue, colorValue, 1);
			damageMatBlock.SetColor("_EmissionColor", newColor);
			damageFlash.SetPropertyBlock(damageMatBlock);
			yield return new WaitForFixedUpdate();
		}
	}

	private IEnumerator DashRoutine(int direction) {
		if (!isDashing) {
			isDashing = true;
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Rigidbody>().drag = 5f;
			GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);
			int timer = 100;
			while (timer > 0) {
				GetComponent<Rigidbody>().AddTorque(Vector3.forward * Time.deltaTime * -1600f * direction);
				timer--;
				yield return new WaitForFixedUpdate();
			}
			GetComponent<Rigidbody>().drag = 0.5f;
			GetComponent<Rigidbody>().AddForce(Vector3.right * 9000f * direction);
			GetComponent<Rigidbody>().useGravity = true;

			//wait a few seconds before it's possible to dash again
			yield return new WaitForSeconds(2.8f);
			isDashing = false;
		}
	}
}
