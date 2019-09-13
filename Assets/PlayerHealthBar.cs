using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour {

	private UnityEngine.UI.Image[] pieces;
	private RectTransform[] pieceTransforms;
	private GameObject followTarget;
	public Camera cam;

	private Vector3 uiPosition;

	public bool[] piecesEnabled;

	// Use this for initialization
	void Start () {
		pieces = new UnityEngine.UI.Image[4];
		pieceTransforms = new RectTransform[4];
		piecesEnabled = new bool[4];

		for (int i = 0; i < 4; i++) {
			piecesEnabled[i] = false;
			pieces[i] = GameObject.Find("Piece" + (i+1)).GetComponent<UnityEngine.UI.Image>();
			pieceTransforms[i] = pieces[i].GetComponent<RectTransform>();
		}

		followTarget = GameObject.FindGameObjectWithTag("Player");
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		cam.ResetWorldToCameraMatrix();
		uiPosition = cam.WorldToScreenPoint(followTarget.transform.position);

		for (int i = 0; i < 4; i++) {
			pieceTransforms[i].SetPositionAndRotation(uiPosition, pieceTransforms[i].rotation);
		}
	}

	public IEnumerator Flash() {
		float timer = 2.5f;

		float alpha = 1f;
		while (timer >= 0f) {
			alpha = timer / 2.5f;
			for (int i = 0; i < 4; i++) {
				if (piecesEnabled[i] == true) {
					pieces[i].color = new Color(1f, 1f, 1f, alpha);
				}
				else
					pieces[i].color = new Color(1f, 1f, 1f, 0f);
			}

			timer -= Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
	}
}
