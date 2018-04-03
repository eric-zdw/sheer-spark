using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitEnemy : Enemy {
	public GameObject healthBar;
	public GameObject explosion;
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
    private MeshRenderer outline;
    private int powerupRoll;

	private bool isCharging;
	private GameObject charge;

    private CameraFollow cam;

    private float moveInterval = 5f;
    private float moveTimer;
    private Vector3 destination;
    public float boundsLowX;
    public float boundsHighX;
    public float boundsLowY;
    public float boundsHighY;
    private float easing;

    private float radius;
    private int layerMask;

    private float attackTime;
    private bool hasAttacked = false;
    private bool isAttacking = false;
    private float flashTimer;
    private float attackDelay = 1.5f;
    private float attackTimer;

    private LineRenderer indicator;
    public GameObject beam;
    private GameObject newBeam;
    private Vector3 attackLocation;

    private float maxSpeed = 20f;

    public GameObject cannonExplosion;

    private Vector3 moveDirection;

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
        outline.material = colours[powerupRoll];
        moveTimer = 0.5f;
        destination = transform.position;
        radius = transform.localScale.y * 1.1f;

        attackTime = moveInterval * 0.4f;

        indicator = GetComponent<LineRenderer>();

        flashTimer = 0f;
        attackTimer = attackDelay;
        hasAttacked = true;

        moveDirection = transform.position;
    }

	void FixedUpdate()
	{
		if (health <= 0)
		{
			Explode();
		}

        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            moveTimer = moveInterval;
            newDirection();
        }

        Vector3 newForce = Vector3.Normalize((moveDirection + player.transform.position) - transform.position) * Time.deltaTime * 100f;
        newForce *= Vector3.Magnitude((moveDirection + player.transform.position) - transform.position);
        rb.AddForce(Vector3.ClampMagnitude(newForce, 12f));
    }

    void newDirection()
    {
        bool isSuccessful = false;
        int attempts = 0;
        Vector3 potentialDirection;
        Vector3 testDirection;
        while (!isSuccessful)
        {
            potentialDirection = Vector3.Normalize(Random.insideUnitCircle) * 8f;
            testDirection = potentialDirection + player.transform.position;
            
            Debug.DrawLine(testDirection, transform.position, Color.red, 0.25f);
            print("moveDirection is " + testDirection.x + ", " + testDirection.y);
            if (testDirection.x > boundsLowX && testDirection.x < boundsHighX
                && testDirection.y > boundsLowY && testDirection.y < boundsHighY)
            {
                isSuccessful = true;
                moveDirection = potentialDirection;
            }
            else
            {
                print("relocation failed! retrying...");
                attempts++;
                if (attempts == 5)
                {
                    print("critical failure! aborting");
                    isSuccessful = true;
                }
            }
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
        cam.addShake(0.5f);
		Instantiate(powerups[powerupRoll], transform.position, Quaternion.identity);
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(bar);
		Destroy(gameObject);
	}
}
