using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour {

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

	}
	
	// Update is called once per frame
	void LateUpdate () {
		uiPosition = camScript.playerScreenPosition;

		for (int i = 0; i < 4; i++) {
			pieceTransforms[i].SetPositionAndRotation(uiPosition, pieceTransforms[i].rotation);
		}

		HPValue = Mathf.Lerp(HPValue, player.HP, 0.1f);

		pieces[0].fillAmount = Mathf.Clamp(HPValue - 3f, 0f, 1f) * 0.25f;
		pieces[1].fillAmount = Mathf.Clamp(HPValue - 2f, 0f, 1f) * 0.25f;
		pieces[2].fillAmount = Mathf.Clamp(HPValue - 1f, 0f, 1f) * 0.25f;
		pieces[3].fillAmount = Mathf.Clamp(HPValue, 0f, 1f) * 0.25f;
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
		
		for (int i = 0; i < 4; i++) {
			pieces[i].color = new Color(1f, 1f, 1f, 0f);
		}
	}
}
