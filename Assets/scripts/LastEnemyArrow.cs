using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastEnemyArrow : MonoBehaviour {

	private GameObject lastEnemy;
	private GameObject player;
	private bool activated = false;
	private Vector3 direction;
	private Camera cam;

	private RectTransform arrowPosition;
	private UnityEngine.UI.Text arrowGraphic;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		arrowPosition = GetComponent<RectTransform>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		arrowGraphic = GetComponent<UnityEngine.UI.Text>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (GameObject.FindGameObjectsWithTag("Enemy").Length == 1) {
			if (!activated) {
				arrowGraphic.enabled = true;
				activated = true;
			}
			lastEnemy = GameObject.FindGameObjectWithTag("Enemy");
			direction = lastEnemy.transform.position - player.transform.position;

			Vector3 uiPosition = cam.WorldToScreenPoint(player.transform.position + (direction * 0.1f));
			float newAngle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
			arrowPosition.SetPositionAndRotation(uiPosition, Quaternion.Euler(0, 0, newAngle));
		}
		else {
			if (activated) {
				arrowGraphic.enabled = false;
				activated = false;
			}
		}
	}
}
