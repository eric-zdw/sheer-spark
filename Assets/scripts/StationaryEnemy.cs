using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemy : Enemy {
	public GameObject healthBar;

	public float newHealth;
	private GameObject bar;

	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

	public GameObject JumpParticleCharge;
	public GameObject JumpParticle;
	public GameObject[] explosions;
	public GameObject[] powerups;
    public Material[] colours;
    private MeshRenderer outline;
    private int powerupRoll;

	private bool isCharging;
	private GameObject charge;

	private NoiseManager noiseManager;

    private float radius;
    private int layerMask;

	private float currentSpeedY = 0f;
	private float currentSpeedX;

	private int moveDirection;
	private float distanceTravelled = 0f;
	public float maxTravelDistance = 3f;
	public int moveSpeed;

    void Start()
	{
		bar = Instantiate(healthBar);
		maxHealth = newHealth;
		health = maxHealth;
		bar.GetComponent<HealthBar>().setTarget(gameObject);

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
		noiseManager = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<NoiseManager>();

        powerupRoll = Random.Range(0, 6);
        outline = transform.GetChild(0).GetComponent<MeshRenderer>();
        outline.material = colours[powerupRoll];
        radius = transform.localScale.y * 1.1f;

		StartCoroutine(Float());

		moveDirection = Random.Range(0, 2);
		if (moveDirection == 0) moveDirection = -1;
		currentSpeedX = moveDirection;
    }

	void FixedUpdate()
	{
		if (health <= 0)
		{
			Explode();
		}

		transform.position += new Vector3(0, currentSpeedY * Time.deltaTime, 0);
		transform.position += new Vector3(currentSpeedX * moveSpeed * Time.deltaTime, 0, 0);
		distanceTravelled += Mathf.Abs(currentSpeedX * moveSpeed * Time.deltaTime);
		if (distanceTravelled >= maxTravelDistance) {
			StartCoroutine(SwitchDirection());
			distanceTravelled -= maxTravelDistance;
		}
		print(distanceTravelled);
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

	IEnumerator Float()
	{
		while (true) {
			while (currentSpeedY <= 0.5f) {
				currentSpeedY += 0.3f * Time.deltaTime;
				yield return null;
			}
			while (currentSpeedY >= -0.5f) {
				currentSpeedY -= 0.3f * Time.deltaTime;
				yield return null;
			}
		}
	}

	IEnumerator SwitchDirection()
	{
		moveDirection *= -1;
		if (moveDirection == -1) {
			while (currentSpeedX >= -1f) {
				currentSpeedX -= 0.5f * Time.deltaTime;
				yield return null;
			}
			currentSpeedX = -1f;
		}
		else {
			while (currentSpeedX <= 1f) {
				currentSpeedX += 0.5f * Time.deltaTime;
				yield return null;
			}
			currentSpeedX = 1f;
		}
	}
}
