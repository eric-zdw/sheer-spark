using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfoText : MonoBehaviour {

	private Camera cam;
	private GameObject player;
	private UnityEngine.UI.Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<UnityEngine.UI.Text>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		print(cam.WorldToViewportPoint(player.transform.position));
	}
}
