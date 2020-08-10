using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public List<GameObject> neighbourNodes;

	public int navX = 0;
	public int navY = 0;

	public enum NodeDirection {UL, U, UR, L, R, DL, D, DR};

	// node attributes
	public bool isGroundNode = false;
	public bool isEdgeNode = false;
	public bool leftEdgeNode = false;
	public bool rightEdgeNode = false;
	public bool isJumpDestination = false;

	public List<Node> jumpConnections;
	public List<Node> dropConnections;
	

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitializeNode(int x, int y) {
		neighbourNodes = new List<GameObject>(8);
		for (int i = 0; i < 8; i++) {
			neighbourNodes.Add(null);
		}

		navX = x;
		navY = y;
	}

	public void MakeJumpConnection(Node dest) {
		jumpConnections.Add(dest);
	}

	public void MakeDropConnection(Node dest) {
		dropConnections.Add(dest);
	}
}
