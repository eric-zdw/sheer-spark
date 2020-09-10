using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemEnemy : Enemy {
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
	private MeshRenderer damageFlash;
    private MeshRenderer outline;
	private MeshRenderer seeThrough;
    private int powerupRoll;

    public bool isTethered;
    private float tetheredCheck = 0.05f;
    private float tetheredTimer;

	private MaterialPropertyBlock damageMatBlock;

	public float pathfindingStrictness = 2f;
	public int iterationsPerFrame = 8;
	public float neighbourAvoidanceWeight = 1f;
	public float neighbourAvoidanceRadius = 5f;

    void Start()
	{
		bar = Instantiate(healthBar);
		maxHealth = newHealth * WaveSystem.enemyPower;
		health = maxHealth;
		bar.GetComponent<HealthBar>().setTarget(gameObject);

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();

		powerupRoll = UnityEngine.Random.Range(0, 6);
		damageFlash = transform.GetChild(2).GetComponent<MeshRenderer>();
        outline = transform.GetChild(0).GetComponent<MeshRenderer>();
		seeThrough = transform.GetChild(1).GetComponent<MeshRenderer>();
        outline.material = colours[powerupRoll];
		seeThrough.material = seeThroughs[powerupRoll];

		damageMatBlock = new MaterialPropertyBlock();
    }

	void FixedUpdate()
	{
		if (health <= 0)
		{
			Explode();
		}

	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
            ExplodeWithoutPowerup();
        }
    }

    void Explode()
	{
		ScoreManager.IncreaseScore(scoreValue);
		Camera.main.GetComponent<CameraFollow>().AddNoise(10f);
		Instantiate(powerups[powerupRoll], transform.position, Quaternion.identity);
		Instantiate(explosions[powerupRoll], transform.position, transform.rotation);
		Destroy(bar);
		Destroy(gameObject);
	}

	void ExplodeWithoutPowerup()
	{
		Camera.main.GetComponent<CameraFollow>().AddNoise(10f);
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
			colorValue -= 5f * Time.deltaTime;
			newColor = new Color(colorValue, colorValue, colorValue, 1);
			damageMatBlock.SetColor("_EmissionColor", newColor);
			damageFlash.SetPropertyBlock(damageMatBlock);
			yield return new WaitForFixedUpdate();
		}
	}
}

