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

	private float currentSpeed = 0f;

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
			while (currentSpeed <= 2f) {
				currentSpeed += 0.1f;
			}
		}

	}
}
