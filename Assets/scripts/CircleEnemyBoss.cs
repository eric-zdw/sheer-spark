using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleEnemyBoss : Enemy {
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

	private float radius;
	private int layerMask;

	private bool isAttacking = false;

	public GameObject[] cannons;

	void Start()
	{
		bar = Instantiate(healthBar);
		maxHealth = newHealth;
		health = maxHealth;
		bar.GetComponent<HealthBar>().setTarget(gameObject);

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();

		for (int i = 0; i <= 4; i++)
		{
			outline = transform.GetChild(i).GetComponent<MeshRenderer>();
			outline.material = colours[0];
		}
		for (int i = 5; i <= 9; i++)
		{
			seeThrough = transform.GetChild(i).GetComponent<MeshRenderer>();
			seeThrough.material = seeThroughs[0];
		}

		radius = transform.localScale.y * 1.1f;

		StartCoroutine(BossRoutine());
	}

	void FixedUpdate()
	{		
		if (health <= 0)
		{
			Explode();
		}

		transform.rotation = Quaternion.Euler(new Vector3 (0f, 0f, transform.rotation.eulerAngles.z + (rotationRate * Time.deltaTime)));
		playerLocation = player.transform.position;

		transform.position = Vector3.Lerp (transform.position, playerLocation, 0.0025f);

		print("rotation rate: " + rotationRate + ", fire rate: " + cannons[0].GetComponent<CircleCannon>().fireRate);
	}

	IEnumerator BossRoutine() {
		float actionTimer = 5f;
		while (true) {
			yield return new WaitForSeconds(1f);
			actionTimer--;

			if (actionTimer <= 0f) {
				StartCoroutine(SpinShootAttack());
				actionTimer = 5f;
				yield return new WaitForSeconds(15f);
			}
		}
	}

	void LaserAttack() {

	}

	IEnumerator SpinShootAttack() {
		bool attacking = true;
		while (attacking) {
			if (rotationRate <= 1000f) {
				rotationRate += 250f * Time.deltaTime;
				for (int i = 0; i < 4; i++) {
					cannons[i].GetComponent<CircleCannon>().fireRate -= 0.02f * Time.deltaTime;
				}
				yield return new WaitForFixedUpdate();
			}
			else {
				yield return new WaitForSeconds(4f);
				while (rotationRate > 50f) {
					rotationRate -= 250f * Time.deltaTime;
					for (int i = 0; i < 4; i++) {
						cannons[i].GetComponent<CircleCannon>().fireRate += 0.02f * Time.deltaTime;
					}
					yield return new WaitForFixedUpdate();
				}
				rotationRate = 50f;
				for (int i = 0; i < 4; i++) {
					cannons[i].GetComponent<CircleCannon>().fireRate = 0.2f;
				}
				attacking = false;
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
		}
	}

	void Explode()
	{
		Camera.main.GetComponent<CameraFollow>().AddNoise(5f);
		Instantiate(powerups[0], transform.position, Quaternion.identity);
		Instantiate(explosions[0], transform.position, transform.rotation);
		Destroy(bar);
		Destroy(gameObject);
	}
}
