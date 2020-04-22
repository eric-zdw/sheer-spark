using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningHealthBar : MonoBehaviour {

	private UnityEngine.UI.Image[] pieces;
	private RectTransform[] pieceTransforms;
	private GameObject followTarget;
	public Camera cam;
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

	public IEnumerator FlashRed() {

		while (player.HP <= 0f) {
			//flash red
			yield return new WaitForSeconds(0.1f);
		}
	}
}
