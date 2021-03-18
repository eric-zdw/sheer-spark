using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DriftEnemy : SmallEnemy {
	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

	public float pathfindingStrictness = 2f;
	public int iterationsPerFrame = 8;
	public float neighbourAvoidanceWeight = 1f;
	public float neighbourAvoidanceRadius = 5f;

	private List<Node> navPath;

	private NavigationType navType = NavigationType.Air;

	private LayerMask playerLOSMask = 1 << 17;

    void Start()
	{
		Initialize();
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
		

		navPath = new List<Node>();
		StartCoroutine(NavigateWrapper());
    }

	private IEnumerator NavigateWrapper() {
		yield return new WaitForSeconds(0.5f);
		Vector3 playerPosition = player.transform.position;
		Vector3 randomOffset = UnityEngine.Random.insideUnitCircle * 8f;
		StartCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition + randomOffset, navType, navPath));
		yield return new WaitForSeconds(1f);
		while (true) {
			// Reset navigation if too far away from navPath, or if player moves to new location
			if ((navPath.Count > 0 && Vector3.Distance(navPath[0].transform.position, transform.position) >= 5f) || Vector3.Distance(player.transform.position, playerPosition) >= 4f) {
				print("enemy too far away from path, recalculating...");
				StopCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition + randomOffset, navType, navPath));
				navPath.Clear();
				playerPosition = player.transform.position;
				//randomOffset = UnityEngine.Random.insideUnitCircle * 2f;
				StartCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition, navType, navPath));
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.375f, 0.625f));
		}
	}

	void FixedUpdate()
	{
		// if enemy has a line of sight to the player, override navigation and travel straight to player
		// at increased speed.
		if (!Physics.Raycast(transform.position, player.transform.position - transform.position, Mathf.Clamp(Vector3.Distance(player.transform.position, transform.position), 0f, 20f), playerLOSMask)) {
			GetComponent<Rigidbody>().AddForce(Vector3.Normalize(player.transform.position - transform.position) * 600f * Time.deltaTime);
		}

		// travel to player via pathfinding if no line of sight.
		else if (navPath.Count > 0) {
			GetComponent<Rigidbody>().AddForce(Vector3.Normalize(navPath[0].transform.position - transform.position) * 300f * Time.deltaTime);
			// For a node to be considered visited, no obstacles must be between the object and the node
			if (Vector3.Distance(navPath[0].transform.position, transform.position) < 2f 
				&& !Physics.Raycast(transform.position, navPath[0].transform.position - transform.position, Vector3.Distance(navPath[0].transform.position, transform.position), LayerMask.NameToLayer("Geometry"))) {
					navPath.RemoveAt(0);
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
}

