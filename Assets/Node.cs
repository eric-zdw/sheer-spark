using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public List<GameObject> neighbourNodes;

	public int navX = 0;
	public int navY = 0;

	public enum NodeDirection {UL, U, UR, L, R, DL, D, DR};

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
}
