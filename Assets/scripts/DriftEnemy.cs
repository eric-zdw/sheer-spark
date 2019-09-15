using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DriftEnemy : Enemy {
	public GameObject healthBar;
	public float newHealth;
	private GameObject bar;

	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

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

	private NoiseManager noiseManager;

    public bool isTethered;
    private float tetheredCheck = 0.05f;
    private float tetheredTimer;

	private MaterialPropertyBlock damageMatBlock;

	public GameObject node;
	private List<GameObject> path;

	private List<List<NodeData>> nodeData;
	//private Dictionary<Tuple<int, int>, NodeData> nodeDictionary; 

	public float pathfindingStrictness = 2f;
	public int iterationsPerFrame = 8;
	public float neighbourAvoidanceWeight = 1f;
	public float neighbourAvoidanceRadius = 5f;

    void Start()
	{
		bar = Instantiate(healthBar);
		maxHealth = newHealth * WaveSystem.enemyPower;
		health = maxHealth;
		bar.GetComponent<HealthBar>().setTarget(gameObject);

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
		noiseManager = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<NoiseManager>();

		powerupRoll = UnityEngine.Random.Range(0, 6);
		damageFlash = transform.GetChild(2).GetComponent<MeshRenderer>();
        outline = transform.GetChild(0).GetComponent<MeshRenderer>();
		seeThrough = transform.GetChild(1).GetComponent<MeshRenderer>();
        outline.material = colours[powerupRoll];
		seeThrough.material = seeThroughs[powerupRoll];

		damageMatBlock = new MaterialPropertyBlock();

		nodeData = new List<List<NodeData>>(200);
		for (int i = 0; i < 200; i++) {
			nodeData.Add(new List<NodeData>(200));
			for (int j = 0; j < 200; j++) {
				nodeData[i].Add(new NodeData(null, 0f, 0f));
			}
		}

		//nodeDictionary = new Dictionary<Tuple<int, int>, NodeData>();

		path = new List<GameObject>();

		StartCoroutine(InitializeNode());
    }

	void FixedUpdate()
	{
		if (health <= 0)
		{
			Explode();
		}

		if (path.Count > 0) {
			rb.AddForce(Vector3.Normalize(path[0].transform.position - transform.position) * 500f * Time.deltaTime);
			if (Vector3.Distance(transform.position, path[0].transform.position) < pathfindingStrictness) {
				path.RemoveAt(0);
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
		noiseManager.AddNoise(10f);
		Instantiate(powerups[powerupRoll], transform.position, Quaternion.identity);
		Instantiate(explosions[powerupRoll], transform.position, transform.rotation);
		Destroy(bar);
		Destroy(gameObject);
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
			colorValue -= 5f * Time.deltaTime;
			newColor = new Color(colorValue, colorValue, colorValue, 1);
			damageMatBlock.SetColor("_EmissionColor", newColor);
			damageFlash.SetPropertyBlock(damageMatBlock);
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator InitializeNode() {
		yield return new WaitForSeconds(1f);
		StartCoroutine(FindPathToPlayer());
	}

	GameObject FindClosestNode(GameObject obj, float radius) {
		Collider[] nodes = Physics.OverlapSphere(obj.transform.position, radius, LayerMask.GetMask("AINode"));
		if (nodes.Length > 0) {
			GameObject choose = null;
			float smallest = Mathf.Infinity;
			for (int i = 0; i < nodes.Length; i++) {
				float distance = Vector3.Distance(obj.transform.position, nodes[i].transform.position);
				if (distance < smallest) {
					smallest = distance;
					choose = nodes[i].gameObject;
				} 
			}

			return choose;
		}
		else {
			print("No node found!");
			return null;
		}
	}

	void OnDrawGizmos() {
		if (node != null) {
			Gizmos.DrawLine(transform.position, node.transform.position);
		}

		if (path.Count > 0) {
			foreach (GameObject pathNode in path) {
				Gizmos.DrawSphere(pathNode.transform.position, 1f);
			}
		}
	}

	private IEnumerator FindPathToPlayer() {
		while (true) {
			List<GameObject> newPath = new List<GameObject>();
			List<GameObject> open = new List<GameObject>();
			HashSet<GameObject> closed = new HashSet<GameObject>();

			GameObject currentNode = FindClosestNode(gameObject, 10f);
			GameObject startingNode = currentNode;
			GameObject targetNode = FindClosestNode(player, 10f);

			Node currentNodeN = currentNode.GetComponent<Node>();
			//print("navX: " + currentNodeN.navX + ", navY: " + currentNodeN.navY);
			nodeData[currentNodeN.navX][currentNodeN.navY].gCost = 0f;
			nodeData[currentNodeN.navX][currentNodeN.navY].hCost = Vector3.Distance(currentNode.transform.position, targetNode.transform.position);

			open.Add(currentNode);

			bool pathFound = false;
			int numberOfIterations = 0;
			float timeStarted = Time.realtimeSinceStartup;
			while (open.Count > 0 && pathFound == false) {
				numberOfIterations++;
				if (numberOfIterations % iterationsPerFrame == 0) {
					//print("Enough iterations, waiting...");
					yield return new WaitForFixedUpdate();
				}

				currentNode = GetLowestCost(open, nodeData);
				currentNodeN = currentNode.GetComponent<Node>();
				open.Remove(currentNode);
				closed.Add(currentNode);

				if (targetNode == currentNode) {
					float timeElapsed = (Time.realtimeSinceStartup - timeStarted);
					print("Path found. Number of iterations: " + numberOfIterations + ", Time elapsed: " + timeElapsed + ", average time per iteration: " + timeElapsed / numberOfIterations);
					pathFound = true;
					RetracePath(startingNode, targetNode);
				}
				else {
					foreach (GameObject neighbour in currentNodeN.neighbourNodes) {
						if (neighbour != null) {
							Node neighbourData = neighbour.GetComponent<Node>();
							if (!closed.Contains(neighbour)) {
								float newCost = nodeData[currentNodeN.navX][currentNodeN.navY].gCost + Vector3.Distance(currentNode.transform.position, neighbour.transform.position);

								if (newCost < nodeData[neighbourData.navX][neighbourData.navY].gCost || !open.Contains(neighbour)) {
									nodeData[neighbourData.navX][neighbourData.navY].gCost = newCost;
									nodeData[neighbourData.navX][neighbourData.navY].hCost = Vector3.Distance(neighbour.transform.position, targetNode.transform.position);
									//float nearbyEnemies = Physics.OverlapSphere(neighbour.transform.position, neighbourAvoidanceRadius, LayerMask.GetMask("Enemy")).Length * neighbourAvoidanceWeight;
									nodeData[neighbourData.navX][neighbourData.navY].fCost = nodeData[neighbourData.navX][neighbourData.navY].gCost + nodeData[neighbourData.navX][neighbourData.navY].hCost; //+ nearbyEnemies;
									nodeData[neighbourData.navX][neighbourData.navY].parent = currentNode;

									if (!open.Contains(neighbour)) {
										open.Add(neighbour);
									}
								}
							}
						}
					}
				}
			}
			yield return new WaitForSeconds(1f);
		}
	}

	/* 
	private IEnumerator FindPathToPlayerDictionary() {
		while (true) {
			List<GameObject> newPath = new List<GameObject>();
			List<GameObject> open = new List<GameObject>();
			HashSet<GameObject> closed = new HashSet<GameObject>();

			GameObject currentNode = FindClosestNode(gameObject, 10f);
			GameObject startingNode = currentNode;
			GameObject targetNode = FindClosestNode(player, 10f);

			Node currentNodeN = currentNode.GetComponent<Node>();
			Tuple<int, int> currentNodeT = new Tuple<int, int>(currentNodeN.navX, currentNodeN.navY);
			nodeDictionary[currentNodeT] = new NodeData();
			nodeDictionary[currentNodeT].gCost = 0f;
			nodeDictionary[currentNodeT].hCost = Vector3.Distance(currentNode.transform.position, targetNode.transform.position);

			open.Add(currentNode);

			bool pathFound = false;
			int numberOfIterations = 0;
			float timeStarted = Time.realtimeSinceStartup;
			while (open.Count > 0 && pathFound == false) {
				numberOfIterations++;
				if (numberOfIterations % iterationsPerFrame == 0) {
					//print("Enough iterations, waiting...");
					yield return new WaitForFixedUpdate();
				}

				currentNode = GetLowestCostDictionary(open);
				currentNodeN = currentNode.GetComponent<Node>();
				currentNodeT = new Tuple<int, int>(currentNodeN.navX, currentNodeN.navY);

				open.Remove(currentNode);
				closed.Add(currentNode);

				if (targetNode == currentNode) {
					float timeElapsed = (Time.realtimeSinceStartup - timeStarted);
					print("Path found. Number of iterations: " + numberOfIterations + ", Time elapsed: " + timeElapsed + ", average time per iteration: " + timeElapsed / numberOfIterations);
					pathFound = true;
					RetracePathDictionary(startingNode, targetNode);
				}
				else {
					foreach (GameObject neighbour in currentNodeN.neighbourNodes) {
						if (neighbour != null) {
							Node neighbourN = neighbour.GetComponent<Node>();
							Tuple<int, int> neighbourT = new Tuple<int, int>(neighbourN.navX, neighbourN.navY);

							if (!closed.Contains(neighbour)) {
								float newCost = nodeDictionary[currentNodeT].gCost + Vector3.Distance(currentNode.transform.position, neighbour.transform.position);

								if (!open.Contains(neighbour) || newCost < nodeDictionary[neighbourT].gCost) {
									nodeDictionary[neighbourT] = new NodeData();
									nodeDictionary[neighbourT].gCost = newCost;
									nodeDictionary[neighbourT].hCost = Vector3.Distance(neighbour.transform.position, targetNode.transform.position);
									nodeDictionary[neighbourT].fCost = newCost + Vector3.Distance(neighbour.transform.position, targetNode.transform.position);
									nodeDictionary[neighbourT].parent = currentNode;

									if (!open.Contains(neighbour)) {
										open.Add(neighbour);
									}
								}
							}
						}
					}
				}
			}
			yield return new WaitForSeconds(1f);
		}
	}
	*/

	private void RetracePath(GameObject start, GameObject target) {
		List<GameObject> newPath = new List<GameObject>();
		GameObject currentNode = target;

		while (currentNode != start) {
			Node currentNodeN = currentNode.GetComponent<Node>();
			newPath.Add(currentNode);
			currentNode = nodeData[currentNodeN.navX][currentNodeN.navY].parent;
		}
		newPath.Reverse();
		path = newPath;
	}

	/* 
	private void RetracePathDictionary(GameObject start, GameObject target) {
		List<GameObject> newPath = new List<GameObject>();
		GameObject currentNode = target;

		while (currentNode != start) {
			Node currentNodeN = currentNode.GetComponent<Node>();
			Tuple<int, int> currentNodeT = new Tuple<int, int>(currentNodeN.navX, currentNodeN.navY);
			newPath.Add(currentNode);
			currentNode = nodeDictionary[currentNodeT].parent;
		}
		newPath.Reverse();
		path = newPath;
	}
	*/

	private GameObject GetLowestCost(List<GameObject> open, List<List<NodeData>> data) {
		float smallestCost = Mathf.Infinity;
		GameObject candidate = null;

		foreach(GameObject openNode in open) {
			float cost = data[openNode.GetComponent<Node>().navX][openNode.GetComponent<Node>().navY].fCost;
			if (cost < smallestCost) {
				smallestCost = cost;
				candidate = openNode;
			}
		}

		return candidate;
	}

	/* 
	private GameObject GetLowestCostDictionary(List<GameObject> open) {
		float smallestCost = Mathf.Infinity;
		GameObject candidate = null;

		foreach(GameObject openNode in open) {
			Node openNodeN = openNode.GetComponent<Node>();
			Tuple<int, int> openNodeT = new Tuple<int, int>(openNodeN.navX, openNodeN.navY);
			float cost = nodeDictionary[openNodeT].fCost;
			if (cost < smallestCost) {
				smallestCost = cost;
				candidate = openNode;
			}
		}

		return candidate;
	}
	*/
}

