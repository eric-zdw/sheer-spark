using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemy : SmallEnemy {
	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

	private bool isCharging;
	private GameObject charge;

    private float moveInterval = 10f;
    private float moveTimer;
    private Vector3 destination;
    public float boundsLowX;
    public float boundsHighX;
    public float boundsLowY;
    public float boundsHighY;
    private float easing;

    private float radius;

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

    void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();

        moveTimer = 1.5f;
        destination = transform.position;
        radius = transform.localScale.y * 1.1f;

        attackTime = moveInterval * 0.4f;

        indicator = GetComponent<LineRenderer>();

        flashTimer = 0f;
        attackTimer = attackDelay;
        hasAttacked = true;
    }

	void FixedUpdate()
	{
        if (easing < 1f)
            easing += Time.deltaTime;
        else if (easing > 1f)
            easing = 1f;

        Vector3 newForce = Vector3.Normalize(destination - transform.position) * Time.deltaTime * 100f * Vector3.Magnitude(destination - transform.position) * easing;
        rb.AddForce(Vector3.ClampMagnitude(newForce, maxSpeed));

        if (!hasAttacked)
        {
            transform.LookAt(player.transform.position);
            indicator.SetPosition(0, player.transform.position + Vector3.Normalize(player.transform.position - transform.position) * 50f);
            indicator.SetPosition(1, transform.position);
        }
        else
        {
            transform.LookAt(attackLocation);
            indicator.SetPosition(0, attackLocation + Vector3.Normalize(attackLocation - transform.position) * 50f);
            indicator.SetPosition(1, transform.position);
        }
            

        Vector3 rotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(rotation.x + 90f, rotation.y, rotation.z);

        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            RelocateRoutine();
            moveTimer = (moveInterval + Random.Range((-moveInterval / 5f), (moveInterval / 5f)));
        }

        if (moveTimer < attackTime && hasAttacked == false)
        {
            hasAttacked = true;
            isAttacking = true;
            attackLocation = player.transform.position;
        }
        if (attackTimer <= 0)
        {
            attackTimer = attackDelay;
            isAttacking = false;
            indicator.enabled = false;
        }

        if(isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer % 0.15f > 0.075f)
            {
                indicator.enabled = true;
            }
            else
                indicator.enabled = false;

            if (attackTimer <= 0f)
            {
                GameObject newExplosion = Instantiate(cannonExplosion, transform.position, transform.rotation);
                newBeam = Instantiate(beam, transform.position, transform.rotation);
                Vector3 newRotation = newBeam.transform.rotation.eulerAngles;
                newBeam.transform.rotation = Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z + 90f);
                Vector3 newForce2 = Vector3.Normalize(attackLocation - transform.position) * Time.deltaTime * 50000f;
                rb.AddForce(-newForce2);
            }
                
        }


	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
            getDamage(50f);
        }
    }

    private void RelocateRoutine()
    {
        easing = 0f;
        int failedAttempts = 0;
        bool success = false;
        while (!success)
        {
            float newLocationX = Random.Range(boundsLowX, boundsHighX);
            float newLocationY = Random.Range(boundsLowY, boundsHighY);
            Vector3 newLocation = new Vector3(newLocationX, newLocationY);

            Debug.DrawLine(transform.position, newLocation, Color.red, 5f);

            if (Physics.OverlapSphere(newLocation, radius).Length != 0)
            {
                print("location attempt at failed! retrying...");
                failedAttempts++;
                if (failedAttempts == 10)
                {
                    print("critical failure! remaining in position");
                    success = true;
                }
            }
            else
            {
                success = true;
                destination = newLocation;
            }
        }
        hasAttacked = false;
    }
}
