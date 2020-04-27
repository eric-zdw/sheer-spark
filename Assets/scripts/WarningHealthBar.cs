using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningHealthBar : MonoBehaviour {

	private UnityEngine.UI.Image[] pieces;
	private RectTransform[] pieceTransforms;
	private GameObject followTarget;
	public Camera cam;
	public CameraFollow camScript;
	public PlayerBehaviour player;

	private float HPValue;

	private Vector3 uiPosition;

	public bool[] piecesEnabled;

	// Use this for initialization
	void Start () {
		pieces = new UnityEngine.UI.Image[4];
		pieceTransforms = new RectTransform[4];
		piecesEnabled = new bool[4];

		for (int i = 0; i < 4; i++) {
			piecesEnabled[i] = true;
			pieces[i] = transform.GetChild(i).GetComponent<UnityEngine.UI.Image>();
			pieceTransforms[i] = pieces[i].GetComponent<RectTransform>();
		}

		followTarget = GameObject.FindGameObjectWithTag("Player");
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		camScript = cam.GetComponent<CameraFollow>();

		StartCoroutine(FlashRed());
	}
	
	// Update is called once per frame
	void LateUpdate () {
		uiPosition = camScript.playerScreenPosition;

		for (int i = 0; i < 4; i++) {
			pieceTransforms[i].SetPositionAndRotation(uiPosition, pieceTransforms[i].rotation);
		}
	}

	public IEnumerator FlashRed() {
		float alpha = 0f;
		bool increasing = true;
		while (true) {
			while (player.HP == 0f) {
				if (increasing) {
					alpha += 2f * Time.deltaTime;
					if (alpha > 0.5f) {
						increasing = false;
					}
				}
				else {
					alpha -= 2f * Time.deltaTime;
					if (alpha < 0f) {
						increasing = true;
					}
				}

				for (int i = 0; i < 4; i++) {
						pieces[i].color = new Color(1f, 0.2f, 0.2f, alpha);
				}			

				yield return new WaitForEndOfFrame();
			}
			alpha = 0f;
			for (int i = 0; i < 4; i++) {
				pieces[i].color = new Color(1f, 0.2f, 0.2f, alpha);
			}	
			yield return new WaitForEndOfFrame();
		}

	}
}
