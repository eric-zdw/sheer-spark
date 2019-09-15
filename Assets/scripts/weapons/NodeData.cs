using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData {
	public GameObject parent;
	public float gCost;
	public float hCost;
	public float fCost;

	public NodeData() {
		this.parent = null;
		this.gCost = 0f;
		this.hCost = 0f;
		this.fCost = 0f;
	}

	public NodeData(GameObject p, float g, float h) {
		this.parent = p;
		this.gCost = g;
		this.hCost = h;
		this.fCost = g + h;
	}

}
