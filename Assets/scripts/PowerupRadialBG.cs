using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupRadialBG : MonoBehaviour {

	private GameObject followTarget;
	private Camera cam;

	private Vector3 uiPosition;

	private RectTransform rect;
	private Image image;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform>();
		image = GetComponent<Image>();

		followTarget = GameObject.Find("MouseObject");
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		uiPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		rect.SetPositionAndRotation(uiPosition, Quaternion.identity);
	}

	
}
