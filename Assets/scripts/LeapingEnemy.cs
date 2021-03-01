using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapingEnemy : SmallEnemy {
	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

	private int layermask;

	private bool isGrounded;
	private float jumpCooldown = 0f;

	private List<Node> navPath;
	private Node currentlyAtNode;
	private bool timeToJump = false;
	private Vector3 playerPosition;

	public float leapDuration = 2f;

	private Color gizColor;

	public GameObject delayedDeathPrefab;

	private bool isLeaping = false;
	private float defaultDrag;

	private NavigationType navType = NavigationType.Jumping;

    void Start()
	{
		Initialize();

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
		defaultDrag = rb.drag;

		layermask = LayerMask.GetMask("Geometry");

		navPath = new List<Node>();
		StartCoroutine(NavigateWrapper());

		rb.maxAngularVelocity = 20f;
    }

	private IEnumerator NavigateWrapper() {
		// wait for initialization
		yield return new WaitForSeconds(0.2f);
		playerPosition = player.transform.position;
		
		StartCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition, navType, navPath));
		yield return new WaitForSeconds(1f);

		//StartCoroutine(TravelToDestination());

		while (true && !isDelayedDeath) {
			// Reset navigation if too far away from navPath, or if player moves to new location
			if (Vector3.Distance(player.transform.position, playerPosition) >= 4f) {
				//print("player moved from target position, recalculating...");
				ResetNavigation();
			}
			if (!timeToJump && Vector3.Distance(currentlyAtNode.transform.position, transform.position) > 5f) {
				//print("enemy strayed too far from path, recalculating...");
				ResetNavigation();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.375f, 0.625f));
		}
	}

	private IEnumerator TravelToDestination() {
		while (true) {
			if (navPath.Count > 0) {
				//print(Vector3.Distance(navPath[0].transform.position, transform.position));

				//---- Check if reached the next node. Update currentlyAtNode and remove from list.
				//---- Also check if currentlyAtNode is a jumping point.

				if (Vector3.Distance(navPath[0].transform.position, transform.position) < 2f 
					&& !Physics.Raycast(transform.position, navPath[0].transform.position - transform.position, Vector3.Distance(navPath[0].transform.position, transform.position), LayerMask.NameToLayer("Geometry"))) {
						currentlyAtNode = navPath[0];
						navPath.RemoveAt(0);
	
					if (currentlyAtNode.jumpConnections.Contains(navPath[0])) {
						timeToJump = true;
					}
					else {
						timeToJump = false;
					}
				}

				//---- Move towards ground node horizontally.
	
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
				
				//---- 
	
				float yDist = navPath[0].transform.position.y - transform.position.y;
				if (isGrounded && jumpCooldown <= 0f && timeToJump && Vector3.Distance(transform.position, currentlyAtNode.transform.position) < 2f && Mathf.Sign(direction) == Mathf.Sign(rb.velocity.x)) {
					if (Vector3.Distance(navPath[0].transform.position, currentlyAtNode.transform.position) > 3f) {
						rb.velocity = new Vector3(rb.velocity.x * 0.5f, 0f, rb.velocity.z);
						LeapToTarget(navPath[0].transform.position);
						//rb.AddForce(new Vector3(direction * 100f, 1500f, 0f));
						jumpCooldown = 2f;
					}
				}
			}
			yield return new WaitForFixedUpdate();
		}
	}

	private void ResetNavigation() {
		StopCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition, navType, navPath));
		navPath.Clear();
		playerPosition = player.transform.position;
		StartCoroutine(NavManager.NavigateToLocation(transform.position, playerPosition, navType, navPath));
	}

	void FixedUpdate()
	{
		if (Physics.Linecast(transform.position, transform.position + Vector3.down * 2f, LayerMask.GetMask("Geometry"))) {
			if (!isGrounded) {
				isGrounded = true;
				isLeaping = false;
				//print("grounded?");
				// If I landed after a jump and didn't seem to reach the node, recalculate route.
				if (timeToJump && Vector3.Distance(transform.position, navPath[0].transform.position) > 3f) {
					print("jump didn't land as intended, recalculating...");
					ResetNavigation();
				}
			}
		}
		else {
			isGrounded = false;
		}

		print(isLeaping);
		if (!isLeaping) {
			if (navPath.Count > 0) {
				print(navPath[0].transform.position);
				//print(Vector3.Distance(navPath[0].transform.position, transform.position));
				if (Vector3.Distance(navPath[0].transform.position, transform.position) < 2f 
					&& !Physics.Raycast(transform.position, navPath[0].transform.position - transform.position, Vector3.Distance(navPath[0].transform.position, transform.position), LayerMask.NameToLayer("Geometry"))) {
						currentlyAtNode = navPath[0];
						navPath.RemoveAt(0);
	
					if (currentlyAtNode.jumpConnections.Count != 0) {
						if (currentlyAtNode.jumpConnections.Contains(navPath[0])) {
							timeToJump = true;
						}
						else {
							timeToJump = false;
						}
					}
				}
	
				if (navPath.Count > 0) {
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
					if (isGrounded && jumpCooldown <= 0f && timeToJump && Vector3.Distance(transform.position, currentlyAtNode.transform.position) < 2f && Mathf.Sign(direction) == Mathf.Sign(rb.velocity.x)) {
						if (Vector3.Distance(navPath[0].transform.position, currentlyAtNode.transform.position) > 3f) {
							rb.velocity = new Vector3(rb.velocity.x * 0.5f, 0f, rb.velocity.z);
							LeapToTarget(navPath[0].transform.position);
							//rb.AddForce(new Vector3(direction * 100f, 1500f, 0f));
							jumpCooldown = 2f;
						}
					}	
				}
			}
	
			jumpCooldown = Mathf.Clamp(jumpCooldown - Time.deltaTime, 0f, 0.5f);
		}
	    
	}

	void OnDrawGizmos() {
		if (navPath != null) {
			foreach (Node n in navPath) {
				Gizmos.DrawCube(n.transform.position, new Vector3(1f, 1f, 1f));
			}
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
			if (collision.gameObject.GetComponent<PlayerBehaviour>().invincible <= 0f) {
				collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
            	getDamage(100f);
			}
        }
    }

	void LeapToTarget(Vector3 target) {
		print("leaping");
		isLeaping = true;
		rb.drag = 0f;

		float g = -Physics.gravity.y;
		float heightOfJump = (target.y - transform.position.y) * 1.5f;

		Vector3 relativeTarget = target - transform.position;

		float speed = 35f;

		float tan1 = (Mathf.Pow(speed, 2) + Mathf.Sqrt(Mathf.Pow(speed, 4) - g * (g * Mathf.Pow(relativeTarget.x, 2) + 2 * relativeTarget.y * Mathf.Pow(speed, 2)))) / (g * relativeTarget.x);
        //float tan2 = (Mathf.Pow(speed, 2) - Mathf.Sqrt(Mathf.Pow(speed, 4) - g * (g * Mathf.Pow(relativeTarget.x, 2) + 2 * relativeTarget.y * Mathf.Pow(speed, 2)))) / (g * relativeTarget.x);
		float launchAngle1 = Mathf.Atan(tan1);
        //float launchAngle2 = Mathf.Atan(tan2);

        //print(launchAngle1 * Mathf.Rad2Deg + ", " + launchAngle2 * Mathf.Rad2Deg);

        Vector3 velocity = new Vector3(Mathf.Cos(launchAngle1), Mathf.Sin(launchAngle1)) * speed;
        print("velocity: " + velocity);

		rb.velocity = Vector3.zero;
		rb.AddForce(velocity, ForceMode.VelocityChange);
		rb.AddTorque(0f, 0f, -50f, ForceMode.VelocityChange);
	}
}
