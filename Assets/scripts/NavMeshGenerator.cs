using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshGenerator : MonoBehaviour {

	public Vector2 floor;
	public Vector2 ceiling;

	//distance between each node
	public float density;

	//amount of space required for a node to be created
	public float padding;

	public GameObject node;
	public static List<List<GameObject>> nodes;
	private LayerMask mask;

	private Node testN;
	private Node testN2;

	public NodeMapScriptableObject nodeMap;

	void Awake () {
		GenerateMesh();
	}

	private void Start () {
		// If NoPathfinding objects were found, disable them before game starts
		print("start");
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("NoPathFinding")) {
			print("found");
			Destroy(obj);
		}
	}

	public void GenerateMesh() {
		CleanUpMesh();
		CreateNodeGrid();
		CheckGroundNode();
		CheckEdgeNode();
		CheckDropConnections();
		StartCoroutine(CheckJumpConnections());
	}
	
	// Update is called once per frame
	void Update () {
		//testN.jumpConnections.Add(testN2);
	}

	void CleanUpMesh() {
		while (transform.childCount != 0) {
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}

	void CreateNodeGrid() {
		//Geometry indicates obstacles
		mask = LayerMask.GetMask("Geometry", "NoPathFinding");

		// create initial node map
		nodes = new List<List<GameObject>>(200);

		for (int i = 0; i < 200; i++) {
			nodes.Add(new List<GameObject>(200));
			for (int j = 0; j < 200; j++) {
				nodes[i].Add(null);
			}
		}



		int iX = 0;
		for (float x = floor.x; x <= ceiling.x; x += density) {
			
			int iY = 0;
			for (float y = floor.y; y <= ceiling.y; y += density) {
				
				Vector3 position = new Vector3(x, y, 0f);

				// don't make a node if there is an obstacle within padding range.
				if (!Physics.CheckSphere(position, padding, mask)) {
					//print("iX: " + iX + ", iY: " + iY);
					//print("Creating node at " + x + " " + y);
					GameObject newNode = Instantiate(node, position, Quaternion.identity, transform);
					newNode.GetComponent<BoxCollider>().size = new Vector3(density, density, 0.1f);
					Node newNodeN = newNode.GetComponent<Node>();
					newNodeN.InitializeNode(iX, iY);
					nodes[iX][iY] = newNode;

					// Y direction is up +'ve, down -'ve
					// e.g. y - 1 corresponds to one node below
					if (iX != 0) {
						if (iY != 0 && nodes[iX - 1][iY - 1] && !ObstacleBetweenNodes(newNode, nodes[iX - 1][iY - 1])) {
							newNodeN.neighbourNodes[(int)Node.NodeDirection.DL] = nodes[iX - 1][iY - 1];
							nodes[iX - 1][iY - 1].GetComponent<Node>().neighbourNodes[(int)Node.NodeDirection.UR] = newNode;
						}
						if (nodes[iX - 1][iY] && !ObstacleBetweenNodes(newNode, nodes[iX - 1][iY])) {
							newNodeN.neighbourNodes[(int)Node.NodeDirection.L] = nodes[iX - 1][iY];
							nodes[iX - 1][iY].GetComponent<Node>().neighbourNodes[(int)Node.NodeDirection.R] = newNode;
						}
						if (iY != 199 && nodes[iX - 1][iY + 1] && !ObstacleBetweenNodes(newNode, nodes[iX - 1][iY + 1])) {
							newNodeN.neighbourNodes[(int)Node.NodeDirection.UL] = nodes[iX - 1][iY + 1];
							nodes[iX - 1][iY + 1].GetComponent<Node>().neighbourNodes[(int)Node.NodeDirection.DR] = newNode;
						}
					}
					if (iY != 0 && nodes[iX][iY - 1] && !ObstacleBetweenNodes(newNode, nodes[iX][iY - 1])) {
						newNodeN.neighbourNodes[(int)Node.NodeDirection.D] = nodes[iX][iY - 1];
						nodes[iX][iY - 1].GetComponent<Node>().neighbourNodes[(int)Node.NodeDirection.U] = newNode;
					}
				}
				else {
					//print("Skipping node at " + x + " " + y);
				}
				iY++;
			}

			iX++;
		}
	}

	bool ObstacleBetweenNodes(GameObject a, GameObject b) {
		Vector3 direction = b.transform.position - a.transform.position;
		if (Physics.Raycast(a.transform.position, direction, Vector3.Magnitude(direction), mask)) {
			return true;
		}
		else return false;
	}

	void CheckGroundNode() {
		for (int i = 0; i < 200; i++) {
			for (int j = 0; j < 200; j++) {
				//check if node is on ground
				if (nodes[i][j] && Physics.Raycast(nodes[i][j].transform.position, Vector3.down, density + padding, LayerMask.GetMask("Geometry"))) {
					nodes[i][j].GetComponent<Node>().isGroundNode = true;
				}
			}
		}
	}

	void CheckEdgeNode() {
		for (int i = 0; i < 200; i++) {
			for (int j = 0; j < 200; j++) {
				//check if node is an edge
				//a node is an edge if it is a ground node AND a side node is not a ground node
				//if a side isn't a ground node but the corresponding diagonal node is (i.e. slopes), the node is not an edge
				if (nodes[i][j] && nodes[i][j].GetComponent<Node>().isGroundNode) {
					if (nodes[i][j].GetComponent<Node>().neighbourNodes[3] && !nodes[i][j].GetComponent<Node>().neighbourNodes[3].GetComponent<Node>().isGroundNode) {
						if (!nodes[i][j].GetComponent<Node>().neighbourNodes[5]) {
							nodes[i][j].GetComponent<Node>().isEdgeNode = true;
							nodes[i][j].GetComponent<Node>().leftEdgeNode = true;
						}
						else if (!nodes[i][j].GetComponent<Node>().neighbourNodes[5].GetComponent<Node>().isGroundNode) {
							nodes[i][j].GetComponent<Node>().isEdgeNode = true;
							nodes[i][j].GetComponent<Node>().leftEdgeNode = true;
						}
					}
					if (nodes[i][j].GetComponent<Node>().neighbourNodes[4] && !nodes[i][j].GetComponent<Node>().neighbourNodes[4].GetComponent<Node>().isGroundNode) {
						if (!nodes[i][j].GetComponent<Node>().neighbourNodes[7]) {
							nodes[i][j].GetComponent<Node>().isEdgeNode = true;
							nodes[i][j].GetComponent<Node>().rightEdgeNode = true;
						}
						else if (!nodes[i][j].GetComponent<Node>().neighbourNodes[7].GetComponent<Node>().isGroundNode) {
							nodes[i][j].GetComponent<Node>().isEdgeNode = true;
							nodes[i][j].GetComponent<Node>().rightEdgeNode = true;
						}
					}
				}
			}
		}
	}

	// Check which nodes that can be dropped to from a node
	// Only applies to edge nodes.
	void CheckDropConnections() {
		for (int i = 0; i < 200; i++) {
			for (int j = 0; j < 200; j++) {
				if (nodes[i][j] && nodes[i][j].GetComponent<Node>().isEdgeNode) {
					//left side
					if (nodes[i][j].GetComponent<Node>().leftEdgeNode) {
						bool groundNodeReached = false;
						try {
							Node currentNode = nodes[i][j].GetComponent<Node>().neighbourNodes[3].GetComponent<Node>().neighbourNodes[3].GetComponent<Node>();
							List<Node> potentialDropNodes = new List<Node>();
							while (!groundNodeReached) {
								if (currentNode.neighbourNodes[6]) {
									potentialDropNodes.Add(currentNode.neighbourNodes[6].GetComponent<Node>());
									currentNode = currentNode.neighbourNodes[6].GetComponent<Node>();

									if (currentNode.isGroundNode) {
										//nodes[i][j].GetComponent<Node>().dropConnections.AddRange(potentialDropNodes);
										nodes[i][j].GetComponent<Node>().dropConnections.Add(currentNode);
										groundNodeReached = true;
									}
								}
								else {
									groundNodeReached = true;
								}
							}
						}
						catch (System.NullReferenceException ex) {
							Debug.Log("drop node missing a drop neighbour, skipping...");
						}
					}
					// Note that nodes could be both left AND right edges.
					if (nodes[i][j].GetComponent<Node>().rightEdgeNode) {
						bool groundNodeReached = false;
						try {
							Node currentNode = nodes[i][j].GetComponent<Node>().neighbourNodes[4].GetComponent<Node>().neighbourNodes[4].GetComponent<Node>();
							List<Node> potentialDropNodes = new List<Node>();
							while (!groundNodeReached) {
								if (currentNode.neighbourNodes[6]) {
									potentialDropNodes.Add(currentNode.neighbourNodes[6].GetComponent<Node>());
									currentNode = currentNode.neighbourNodes[6].GetComponent<Node>();

									if (currentNode.isGroundNode) {
										//nodes[i][j].GetComponent<Node>().dropConnections.AddRange(potentialDropNodes);
										nodes[i][j].GetComponent<Node>().dropConnections.Add(currentNode);
									}
								}
								else {
									groundNodeReached = true;
								}
							}
						}
						catch (System.NullReferenceException ex) {
							Debug.Log("drop node missing a drop neighbour, skipping...");
						}
					}
				}
			}
		}
	}

	// Check which nodes that can be reached by jumping.
	// Jumps are indicated manually by adding JumpNodes.
	// Only applies to ground nodes.
	IEnumerator CheckJumpConnections() {
		yield return new WaitForSeconds(0.5f);
		GameObject jumpNodes = GameObject.FindGameObjectWithTag("JumpNodes");
		if (jumpNodes != null) {
			foreach (Transform jumpSet in jumpNodes.transform) {
				// index 0 is start, index 1 is end
				testN = FindClosestNode(jumpSet.GetChild(0).position);
				testN2 = FindClosestNode(jumpSet.GetChild(1).position);
				testN.jumpConnections.Add(testN2);
				testN2.isJumpDestination = true;
				//print(testN.transform.position);
				//print(testN.jumpConnections.Count);
			}
		}
	}

	void OnDrawGizmosSelected() {
		if (nodes != null) {
			for (int i = 0; i < 200; i++) {
				for (int j = 0; j < 200; j++) {
					if (nodes[i][j]) {
						for (int k = 0; k < 8; k++) {
							if (nodes[i][j].GetComponent<Node>().neighbourNodes[k]) {
								Gizmos.DrawLine(nodes[i][j].transform.position, nodes[i][j].GetComponent<Node>().neighbourNodes[k].transform.position);
							}
						}
						if (nodes[i][j].GetComponent<Node>().isGroundNode) {
							Gizmos.color = Color.green;
							Gizmos.DrawSphere(nodes[i][j].transform.position, 0.4f);

							foreach (Node n in nodes[i][j].GetComponent<Node>().jumpConnections) {
								Gizmos.color = Color.green;
								Gizmos.DrawLine(nodes[i][j].transform.position, n.transform.position);
							}
						}
						if (nodes[i][j].GetComponent<Node>().isEdgeNode) {
							Gizmos.color = Color.magenta;
							Gizmos.DrawSphere(nodes[i][j].transform.position, 0.6f);

							foreach (Node n in nodes[i][j].GetComponent<Node>().dropConnections) {
								Gizmos.color = Color.magenta;
								Gizmos.DrawLine(nodes[i][j].transform.position, n.transform.position);
							}
						}
						Gizmos.color = Color.white;
					}
				}
			}			
		}
		
	}

    private static Node FindClosestNode(Vector3 pos) {

        float smallestDistance = Mathf.Infinity;
        Node n = null;

        foreach (List<GameObject> row in nodes) {
            foreach (GameObject node in row) {
                if (node) {
                    float dist = Vector3.Distance(node.transform.position, pos);
                    if (dist < smallestDistance) {
                        smallestDistance = dist;
                        n = node.GetComponent<Node>();
                    }
                }
            }
        }
        
        return n;
    }
}
