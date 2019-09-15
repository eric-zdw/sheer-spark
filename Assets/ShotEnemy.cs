﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEnemy : Enemy {
	public GameObject healthBar;
	public GameObject[] explosions;
	public float newHealth;
	private GameObject bar;

	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

	public float rotationRate;

	public GameObject[] powerups;
	public Material[] colours;
	public Material[] seeThroughs;
	private MeshRenderer outline;
	private MeshRenderer seeThrough;
	private int powerupRoll;

	private bool isCharging;
	private GameObject charge;

	private NoiseManager noiseManager;

	private float radius;
	private int layerMask;

	public float moveForce;

	void Start()
	{
		bar = Instantiate(healthBar);
		maxHealth = newHealth * WaveSystem.enemyPower;
		health = maxHealth;
		bar.GetComponent<HealthBar>().setTarget(gameObject);

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
		noiseManager = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<NoiseManager>();

		powerupRoll = Random.Range(0, 6);
		for (int i = 0; i <= 4; i++)
		{
			outline = transform.GetChild(i).GetComponent<MeshRenderer>();
			outline.material = colours[powerupRoll];
		}
		for (int i = 5; i <= 9; i++)
		{
			seeThrough = transform.GetChild(i).GetComponent<MeshRenderer>();
			seeThrough.material = seeThroughs[powerupRoll];
		}

		radius = transform.localScale.y * 1.1f;
	}

	void FixedUpdate()
	{		
		if (health <= 0)
		{
			Explode();
		}

		transform.LookAt(player.transform.position);
		Vector3 targetPosition = player.transform.position + Vector3.Normalize(transform.position - player.transform.position) * 16f;
		rb.AddForce((targetPosition - transform.position) * moveForce * Time.deltaTime);
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
		noiseManager.AddNoise(5f);
		Instantiate(powerups[powerupRoll], transform.position, Quaternion.identity);
		Instantiate(explosions[powerupRoll], transform.position, transform.rotation);
		Destroy(bar);
		Destroy(gameObject);
	}
}
