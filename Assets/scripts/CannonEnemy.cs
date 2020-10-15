using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemy : SmallEnemy {
	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

    private List<Node> navPath;

	public GameObject body;
	public GameObject cannon;

	public LineRenderer laserPointer;

	private float dampVelocity = 0f;

    void Start()
	{
        Initialize();
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();

        navPath = new List<Node>();
        StartCoroutine(NavigateWrapper());

		StartCoroutine(AttackRoutine());
    }

	void FixedUpdate()
	{
        if (navPath.Count > 0) {
			GetComponent<Rigidbody>().AddForce(Vector3.Normalize(navPath[0].transform.position - transform.position) * 600f * Time.deltaTime);
			// For a node to be considered visited, no obstacles must be between the object and the node
			if (Vector3.Distance(navPath[0].transform.position, transform.position) < 2f 
				&& !Physics.Raycast(transform.position, navPath[0].transform.position - transform.position, Vector3.Distance(navPath[0].transform.position, transform.position), LayerMask.NameToLayer("Geometry"))) {
					navPath.RemoveAt(0);
			}
		}

		/*
		Vector2 currentDirection = cannon.transform.position - body.transform.position;
		float currentAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
		*/

		Vector2 targetDirection = player.transform.position - body.transform.position;
		float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
		
		float newAngle = Mathf.SmoothDampAngle(cannon.transform.eulerAngles.z - 90f, targetAngle, ref dampVelocity, 0.8f);

		cannon.transform.rotation = Quaternion.Euler(new Vector3(cannon.transform.rotation.eulerAngles.x, 0f, newAngle + 90f));
		cannon.transform.position = body.transform.position - cannon.transform.up * 1.5f;

		if (laserPointer.enabled) {
			laserPointer.SetPosition(0, cannon.transform.position);
			laserPointer.SetPosition(1, cannon.transform.position + Vector3.Normalize(cannon.transform.position - transform.position) * 100f);
		}
	}

    private IEnumerator NavigateWrapper() {
		yield return new WaitForSeconds(0.5f);
		while (true) {
			Vector3 playerPosition = player.transform.position;
			Vector3 randomOffset = UnityEngine.Random.insideUnitCircle * 50f;
			StartCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition + randomOffset, false, navPath));
			yield return new WaitForSeconds(16f);
			StopCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition + randomOffset, false, navPath));
			navPath.Clear();
		}
	}

	private IEnumerator AttackRoutine() {
		while (true) {
			StartCoroutine(LaserFlash());
			yield return new WaitForSeconds(10f);
		}
	}

	private IEnumerator LaserFlash() {
		float flashInterval = 0.2f;
		while (flashInterval >= 0.04f) {
			laserPointer.enabled = true;
			laserPointer.SetPosition(0, cannon.transform.position);
			laserPointer.SetPosition(1, cannon.transform.position + Vector3.Normalize(cannon.transform.position - transform.position) * 100f);
			yield return new WaitForSeconds(flashInterval);
			flashInterval *= 0.95f;
			laserPointer.enabled = false;
			yield return new WaitForSeconds(flashInterval * 0.5f);
			flashInterval *= 0.95f;
		}
		//if Physics.SphereCast()
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
            getDamage(50f);
        }
    }

	void OnDrawGizmosSelected() {
		foreach (Node n in navPath) {
			Gizmos.DrawCube(n.transform.position, new Vector3(2f, 2f, 2f));
		}
		
	}
}
