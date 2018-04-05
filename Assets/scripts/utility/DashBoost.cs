using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoost : Utility {

    private GameObject player;
    private Rigidbody rb;
    private Camera cam;
    private CameraFollow camFollow;
    private Vector2 mousePosition;
    private float angle;

    public GameObject dashImpact;
    public float impactDamage = 50f;
    private Enemy targetEnemy;

    public float dashVelocity = 15f;
    bool isPrimed = false;

    public float charges;
    private AudioSource[] sounds;

    private float dashDelay = 0.05f;
    private float dashTimer;

    // Use this for initialization
    void Start () {
        SetUseRate(useRate);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camFollow = cam.GetComponent<CameraFollow>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
        sounds = GetComponents<AudioSource>();
        dashTimer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camFollow.CameraDistance));

        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
        }

        if (charges < 3f)
        {
            charges += 0.75f * Time.deltaTime;
        }
        else if (charges > 3f)
            charges = 3f;
    }

    public override void Activate()
    {
        if (charges >= 1f && dashTimer <= 0f)
        {
            angle = Mathf.Atan2(mousePosition.y - player.transform.position.y, mousePosition.x - player.transform.position.x);
            rb.velocity = new Vector3(dashVelocity * Mathf.Cos(angle), dashVelocity * Mathf.Sin(angle), 0f);
            if (rb.velocity.x > 0f)
                rb.angularVelocity = new Vector3(0f, 0f, -10f);
            else
                rb.angularVelocity = new Vector3(0f, 0f, 10f);

            Instantiate(dashImpact, transform.position, transform.rotation);
            charges -= 1f;
            sounds[0].Play();
            dashTimer = dashDelay;
        }
        else if (charges < 1f && dashTimer <= 0f)
        {
            sounds[1].Play();
        }
    }

	/*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Instantiate(dashImpact, transform.position, transform.rotation);
            targetEnemy = other.GetComponent<Enemy>();
            targetEnemy.getDamage(impactDamage);
        }
    }
    */
}
