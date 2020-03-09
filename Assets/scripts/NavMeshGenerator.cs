using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshGenerator : MonoBehaviour {

	public Vector2 floor;
	public Vector2 ceiling;
	public float density;
	public float padding;

	public GameObject node;
	private List<List<GameObject>> nodes;
	private LayerMask mask;

	// Use this for initialization
	void Start () {
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
						if (iY != 0 && nodes[iX - 1][iY - 1]) {
							newNodeN.neighbourNodes[(int)Node.NodeDirection.DL] = nodes[iX - 1][iY - 1];
							nodes[iX - 1][iY - 1].GetComponent<Node>().neighbourNodes[(int)Node.NodeDirection.UR] = newNode;
						}
						if (nodes[iX - 1][iY]) {
							newNodeN.neighbourNodes[(int)Node.NodeDirection.L] = nodes[iX - 1][iY];
							nodes[iX - 1][iY].GetComponent<Node>().neighbourNodes[(int)Node.NodeDirection.R] = newNode;
						}
						if (iY != 199 && nodes[iX - 1][iY + 1]) {
							newNodeN.neighbourNodes[(int)Node.NodeDirection.UL] = nodes[iX - 1][iY + 1];
							nodes[iX - 1][iY + 1].GetComponent<Node>().neighbourNodes[(int)Node.NodeDirection.DR] = newNode;
						}
					}
					if (iY != 0 && nodes[iX][iY - 1]) {
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
	
	// Update is called once per frame
	void Update () {
		
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
					}
				}
			}			
		}
		
	}
}
