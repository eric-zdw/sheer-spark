using System.Collections;
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

	private int layermask;

	private bool isGrounded;
	private float jumpCooldown = 0f;

	private List<Node> navPath;
	private Node currentNode;
	private bool timeToJump = false;
	private Vector3 playerPosition;

	private Color gizColor;

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

		layermask = LayerMask.GetMask("Geometry");

		navPath = new List<Node>();
		StartCoroutine(NavigateWrapper());

		damageMatBlock = new MaterialPropertyBlock();

		rb.maxAngularVelocity = 10f;
    }

	private IEnumerator NavigateWrapper() {
		yield return new WaitForSeconds(0.5f);
		playerPosition = player.transform.position;
		StartCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition, true, navPath));
		yield return new WaitForSeconds(1f);

		while (true) {
			// Reset navigation if too far away from navPath, or if player moves to new location
			if (Vector3.Distance(player.transform.position, playerPosition) >= 4f) {
				print("player moved from target position, recalculating...");
				ResetNavigation();
			}
			if (!timeToJump && Vector3.Distance(currentNode.transform.position, transform.position) > 5f) {
				print("enemy strayed too far from path, recalculating...");
				ResetNavigation();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.375f, 0.625f));
		}
	}

	private void ResetNavigation() {
		StopCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition, true, navPath));
		navPath.Clear();
		playerPosition = player.transform.position;
		StartCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition, true, navPath));
	}

	void FixedUpdate()
	{
		if (health <= 0)
		{
			Explode();
		}

		Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red, 0.1f);
		if (Physics.Linecast(transform.position, transform.position + Vector3.down * 2f, LayerMask.GetMask("Geometry"))) {
			if (!isGrounded) {
				isGrounded = true;
				print("grounded?");
				// If I presumably landed after a jump and didn't seem to reach the node, recalculate route.
				if (timeToJump && Vector3.Distance(transform.position, navPath[0].transform.position) > 3f) {
					print("jump didn't land as intended, recalculating...");
					ResetNavigation();
				}
			}
		}
		else {
			isGrounded = false;
		}
		
		/*
        if (player != null)
        {
            playerLocation = player.transform.position;

            if (transform.position.x < playerLocation.x && rb.velocity.x < 9f)
            {
                rb.AddForce(new Vector3(1000f, 0f, 0f) * Time.deltaTime);
                rb.AddTorque(0, 0, -80f * Time.deltaTime);
            }
            else if (transform.position.x > playerLocation.x && rb.velocity.x > -9f)
            {
                rb.AddForce(new Vector3(-1000f, 0f, 0f) * Time.deltaTime);
                rb.AddTorque(0, 0, 80f * Time.deltaTime);
            }
        }
		*/

		if (navPath.Count > 0) {
			//print(Vector3.Distance(navPath[0].transform.position, transform.position));
			if (Vector3.Distance(navPath[0].transform.position, transform.position) < 2f 
				&& !Physics.Raycast(transform.position, navPath[0].transform.position - transform.position, Vector3.Distance(navPath[0].transform.position, transform.position), LayerMask.NameToLayer("Geometry"))) {
					currentNode = navPath[0];
					navPath.RemoveAt(0);

				if (currentNode.jumpConnections.Contains(navPath[0])) {
					timeToJump = true;
				}
				else {
					timeToJump = false;
				}
			}

			float xDist = navPath[0].transform.position.x - transform.position.x;
			int direction = 0;
			if (xDist > 0f) {
				direction = 1;
				//print("right");
                rb.AddForce(new Vector3(350f, 0f, 0f) * Time.deltaTime);
                rb.AddTorque(0, 0, -100f * Time.deltaTime);
			}
			else if (xDist < 0f){
				direction = -1;
				//print("left");
                rb.AddForce(new Vector3(-350f, 0f, 0f) * Time.deltaTime);
                rb.AddTorque(0, 0, 100f * Time.deltaTime);
			}

			

			float yDist = navPath[0].transform.position.y - transform.position.y;
			if (isGrounded && jumpCooldown <= 0f && timeToJump && Vector3.Distance(transform.position, currentNode.transform.position) < 2f && Mathf.Sign(direction) == Mathf.Sign(rb.velocity.x)) {
				if (Vector3.Distance(navPath[0].transform.position, currentNode.transform.position) > 3f) {
					rb.velocity = new Vector3(rb.velocity.x * 0.5f, 0f, rb.velocity.z);
					rb.AddForce(new Vector3(direction * 100f, 1500f, 0f));
					jumpCooldown = 2f;
				}
			}
		}

		jumpCooldown = Mathf.Clamp(jumpCooldown - Time.deltaTime, 0f, 0.5f);

		rb.AddForce(new Vector3(-rb.velocity.x, 0, 0) * 20f * Time.deltaTime);
		rb.AddForce(new Vector3(0, -rb.velocity.y, 0) * 50f * Time.deltaTime);


		//-------------------

		//CheckLinecastCollision();
		//JumpRoutine();

        //-------------------
	    
	}

	/*
	void OnDrawGizmos() {
		foreach (Node n in navPath) {
			Gizmos.DrawCube(n.transform.position, new Vector3(1f, 1f, 1f));
		}
	}
	*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
			if (collision.gameObject.GetComponent<PlayerBehaviour>().invincible <= 0f) {
				collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
            	ExplodeWithoutPowerup();
			}
        }
    }

    void Explode()
	{
		ScoreManager.IncreaseScore(score);
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

	void JumpRoutine()
	{
		if (jumpTimer > 0)
			jumpTimer -= Time.deltaTime;
		else
			isJumping = false;


		if (isJumping == false) 
		{
			jumpCounter = Random.Range(1, 1000);
			if (jumpCounter <= jumpChance && transform.position.y <= YLimit)
			{
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

	public override void getDamage(float damage)
    {
        health -= damage;
		StartCoroutine(FlashWhite());
    }

	IEnumerator FlashWhite() {
		float colorValue = 2f;
		Color newColor = new Color(colorValue, colorValue, colorValue, 1);
		while (colorValue > 0f) {
			print("colorValue: " + colorValue);
			colorValue -= 5f * Time.deltaTime;
			newColor = new Color(colorValue, colorValue, colorValue, 1);
			damageMatBlock.SetColor("_EmissionColor", newColor);
			damageFlash.SetPropertyBlock(damageMatBlock);
			yield return new WaitForFixedUpdate();
		}
	}
}
