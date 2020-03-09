using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupRadial : MonoBehaviour {

	private GameObject player;
	private GameObject followTarget;
	private PlayerBehaviour playerBehaviour;
	public Camera cam;

	private Vector3 uiPosition;

	private RectTransform rect;
	private Image image;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform>();
		image = GetComponent<Image>();

		player = GameObject.FindGameObjectWithTag("Player");
		playerBehaviour = player.GetComponent<PlayerBehaviour>();

		followTarget = GameObject.Find("MouseObject");

		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		cam.ResetWorldToCameraMatrix();
		uiPosition = cam.WorldToScreenPoint(followTarget.transform.position);
		rect.SetPositionAndRotation(uiPosition, Quaternion.identity);

		image.fillAmount = (playerBehaviour.powerupTimer / playerBehaviour.powerupDuration);
	}

	public void changePowerup(Material mat) {
		image.material = mat;
	}
}
