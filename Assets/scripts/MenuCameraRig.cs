using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraRig : MonoBehaviour {

	public Vector3[] positions;
	public Vector3[] rotations;
	public static int currentPosition = 0;
	public float transitionRate = 0.05f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Vector3.Lerp(transform.position, positions[currentPosition], transitionRate);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotations[currentPosition]), transitionRate);
	}

	public static void ChangePosition(int index) {
		currentPosition = index;
	}

	public void ChangePositionInstance(int index) {
		ChangePosition(index);
	}
}
