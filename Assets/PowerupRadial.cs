using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupRadial : MonoBehaviour {

	public GameObject player;
	private PlayerBehaviour playerBehaviour;
	public Camera cam;

	private Vector3 uiPosition;

	private RectTransform rect;
	private Image image;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform>();
		image = GetComponent<Image>();

		playerBehaviour = player.GetComponent<PlayerBehaviour>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		cam.ResetWorldToCameraMatrix();
		uiPosition = cam.WorldToScreenPoint(player.transform.position);
		rect.SetPositionAndRotation(uiPosition, Quaternion.identity);

		image.fillAmount = (playerBehaviour.powerupTimer / playerBehaviour.powerupDuration);
	}

	public void changePowerup(Material mat) {
		image.material = mat;
	}
}
