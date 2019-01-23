using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimManager : MonoBehaviour {

	public float magnitude;

	private Camera cam;
	private Cinemachine.CinemachineTransposer transposer;
	Vector3 mousePosition;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		transposer = GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineTransposer>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 finalOffset = cam.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 15f)) - transform.position;
		print (finalOffset);
		transposer.m_FollowOffset = new Vector3(finalOffset.x, finalOffset.y, -15f);
	}
}
