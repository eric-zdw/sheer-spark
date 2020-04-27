using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleBoost : Utility {

    private GameObject player;
    private PlayerBehaviour playerBehaviour;
    private Rigidbody rb;
    private Camera cam;
    private CameraFollow camFollow;
    private Vector2 mousePosition;
    private float angle;

    public GameObject dashImpact;
    public float impactDamage = 50f;
    private Enemy targetEnemy;

    public float dashVelocity = 20f;
    public float dashHeatVelocity = 20f;
    public float teleDistance = 4f;
    bool isPrimed = false;

    public float charges;
    private AudioSource[] sounds;

    private float dashDelay = 0.05f;
    private float dashTimer;

    private int layermask = ~(1 << 9 | 1 << 13 | 1 << 8 | 1 << 14 | 1 << 18 | 1 << 16);

    // Use this for initialization
    void Start () {
        SetUseRate(useRate);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camFollow = cam.GetComponent<CameraFollow>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
        rb = player.GetComponent<Rigidbody>();
        sounds = GetComponents<AudioSource>();
        dashTimer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camFollow.CameraDistance));
        print(camFollow.CameraDistance);

        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
        }

        if (charges < 4f)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1f, layermask)) {
                charges += 10f * Time.deltaTime;
            }
            else {
                charges += 0.25f * Time.deltaTime;
            }
        }
        else if (charges > 4f)
            charges = 4f;

		//print ("horizontal: " + Input.GetAxis ("Horizontal") + ", vertical: " + Input.GetAxis ("Vertical"));
    }

    public override void Activate()
    {
        StopCoroutine(SlowDown());

        if (charges >= 1f && dashTimer <= 0f)
        {
            angle = Mathf.Atan2(mousePosition.y - player.transform.position.y, mousePosition.x - player.transform.position.x);
			//angle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

            float dashRealVelocity = dashVelocity + (dashHeatVelocity * playerBehaviour.heatFactor);
            //player.transform.position += new Vector3(teleDistance * Mathf.Cos(angle), teleDistance * Mathf.Sin(angle), 0f);
            rb.velocity = new Vector3(dashRealVelocity * Mathf.Cos(angle) * 2f, dashRealVelocity * Mathf.Sin(angle), 0f) * 2f;
            if (rb.velocity.x > 0f)
                rb.angularVelocity = new Vector3(0f, 0f, -4f);
            else
                rb.angularVelocity = new Vector3(0f, 0f, 4f);

            Instantiate(dashImpact, transform.position, transform.rotation);
            charges -= 2f;
            sounds[0].Play();
            dashTimer = dashDelay;
            StartCoroutine(SlowDown());
        }
        else if (charges < 1f && dashTimer <= 0f)
        {
            sounds[1].Play();
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && playerBehaviour.isDashing)
        {
            Instantiate(dashImpact, transform.position, transform.rotation);
            targetEnemy = other.GetComponent<Enemy>();
            targetEnemy.getDamage(impactDamage);
        }
    }

    public IEnumerator SlowDown() {
        playerBehaviour.isDashing = true;
        Physics.IgnoreLayerCollision(8, 16, true);
        rb.useGravity = false;
        for (int i = 0; i < 20; i++) {
            rb.velocity *= 0.88f;
            yield return new WaitForFixedUpdate();
        }
        playerBehaviour.isDashing = false;
        Physics.IgnoreLayerCollision(8, 16, false);
        rb.useGravity = true;
    }
}
