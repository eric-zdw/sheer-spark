using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEnemy : SmallEnemy {
	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

	public float rotationRate;
	private bool isCharging;
	private GameObject charge;

	private float radius;
	private int layerMask;
	public float moveForce;

	void Start()
	{
		Initialize();
		MeshRenderer cannonMesh = transform.GetChild(3).GetComponent<MeshRenderer>();
		cannonMesh.material = smallEnemyData.outlines[powerupRoll];
		MeshRenderer cannonSeeThrough = transform.GetChild(4).GetComponent<MeshRenderer>();
		cannonSeeThrough.material = smallEnemyData.seeThroughMats[powerupRoll];

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();

		radius = transform.localScale.y * 1.1f;
	}

	void FixedUpdate()
	{
		transform.LookAt(player.transform.position);
		Vector3 targetPosition = player.transform.position + Vector3.Normalize(transform.position - player.transform.position) * 16f;
		rb.AddForce((targetPosition - transform.position) * moveForce * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
			getDamage(50f);
		}
	}
}
