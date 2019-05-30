using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateEnemyPair : MonoBehaviour {

	private LineRenderer lineRenderer;
	public GameObject enemy1;
	public GameObject enemy2;

	public Rigidbody rb1;
	public Rigidbody rb2;

	private Vector3 desiredPos1;
	private Vector3 desiredPos2;
	private Vector3 desiredPosBase;

	public ParticleSystem particles;

	public int powerupRoll;

	private float radians1 = 0f;
	private float radians2 = Mathf.PI;

	public Material[] colours;
	public Color[] particleColours;

	private GameObject player;

	private int layermask = ~(1 << 9 | 1 << 13 | 1 << 14);

	private float cooldown = 0f;

	// Use this for initialization
	void Start () {
		powerupRoll = Random.Range(0, 6);
		print("poweruproll: " + powerupRoll);
		Vector3 enemyPos1 = enemy1.transform.position;
		Vector3 enemyPos2 = enemy2.transform.position;
		desiredPosBase = (enemyPos1 + enemyPos2) / 2f;

		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.startWidth = 0.2f;
		lineRenderer.material = colours[powerupRoll];

		ParticleSystem.MainModule particleMain = particles.main;
		particleMain.startColor = particleColours[powerupRoll];

		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if ((enemy1 == null) || (enemy2 == null)) {
			particles.enableEmission = false;
			lineRenderer.enabled = false;
		}
		else {
			radians1 += Time.deltaTime * 1f;
			radians2 += Time.deltaTime * 1f;
			Vector3 enemyPos1 = enemy1.transform.position;
			Vector3 enemyPos2 = enemy2.transform.position;
			Vector3 direction = enemyPos1 - enemyPos2;
			float distance = Vector3.Distance(enemyPos1, enemyPos2);

			lineRenderer.SetPosition(0, enemyPos1);
			lineRenderer.SetPosition(1, enemyPos2);

			ParticleSystem.ShapeModule shape = particles.shape;
			shape.radius = distance / 2f;
			shape.rotation = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
			shape.position = transform.InverseTransformPoint(Vector3.Lerp(enemyPos1, enemyPos2, 0.5f));

			desiredPosBase = Vector3.Lerp(desiredPosBase, player.transform.position, 0.005f);

			Vector3 desiredRotation1 = new Vector3(Mathf.Cos(radians1), Mathf.Sin(radians1), 0f);
			desiredRotation1 *= 8f;
			desiredPos1 = desiredPosBase + desiredRotation1;
			rb1.AddForce((desiredPos1 - enemy1.transform.position) * 50f * Time.deltaTime);

			Vector3 desiredRotation2 = new Vector3(Mathf.Cos(radians2), Mathf.Sin(radians2), 0f);
			desiredRotation2 *= 8f;
			desiredPos2 = desiredPosBase + desiredRotation2;
			rb2.AddForce((desiredPos2 - enemy2.transform.position) * 50f * Time.deltaTime);

			RaycastHit info;
        	if (Physics.Linecast(enemyPos1, enemyPos2, out info, layermask) && cooldown <= 0f) {
            	if (info.collider.tag == "Player") {
					cooldown = 1f;
					info.collider.GetComponent<PlayerBehaviour>().takeDamage(1);
				}
        	}

			if ((enemy1 == null) && (enemy2 == null)) {
				Destroy(gameObject, 2f);
			}

			if (cooldown > 0f) cooldown -= Time.deltaTime;
		}
		

		//Debug.DrawLine(enemy1.transform.position, desiredPos1, Color.red, 0.1f);
		//Debug.DrawLine(enemy1.transform.position, desiredPos1, Color.blue, 0.1f);

		
	}

	void CheckLinecastCollision() {

    }
}
