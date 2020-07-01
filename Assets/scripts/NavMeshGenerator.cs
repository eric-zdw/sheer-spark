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

	void Start () {
		CreateNodeGrid();
		CheckGroundNode();
		CheckEdgeNode();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CreateNodeGrid() {
		//Geometry indicates obstacles
		mask = LayerMask.GetMask("Geometry");
		
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

				if (!Physics.CheckSphere(position, padding, mask)) {
					//print("iX: " + iX + ", iY: " + iY);
					//print("Creating node at " + x + " " + y);
					GameObject newNode = Instantiate(node, position, Quaternion.identity, transform);
					newNode.GetComponent<BoxCollider>().size = new Vector3(density, density, 0.1f);
					Node newNodeN = newNode.GetComponent<Node>();
					newNodeN.InitializeNode(iX, iY);
					nodes[iX][iY] = newNode;

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
		if (Physics.Raycast(a.transform.position, direction, Vector3.Magnitude(direction), LayerMask.GetMask("Geometry"))) {
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

	void CheckDropConnections() {
		//Check if it is safe to drop from this node, and to which node it drops to
		//Only applies to edge nodes

		for (int i = 0; i < 200; i++) {
			for (int j = 0; j < 200; j++) {
				if (nodes[i][j] && nodes[i][j].GetComponent<Node>().isEdgeNode) {
					//left side
					if (nodes[i][j].GetComponent<Node>().leftEdgeNode) {
						bool groundNodeReached = false;
						Node currentNode = nodes[i][j].GetComponent<Node>().neighbourNodes[3].GetComponent<Node>();
						while (!groundNodeReached) {
							//todo
							//currentNode
						}
					}
				}
			}
		}
	}

	void CheckJumpConnections() {
		
	}

	void OnDrawGizmos() {
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
						}
						if (nodes[i][j].GetComponent<Node>().isEdgeNode) {
							Gizmos.color = Color.magenta;
							Gizmos.DrawSphere(nodes[i][j].transform.position, 0.6f);
						}
						Gizmos.color = Color.white;
					}
				}
			}			
		}
		
	}
}
