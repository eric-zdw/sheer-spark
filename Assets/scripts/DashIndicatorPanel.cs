using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIndicatorPanel : MonoBehaviour {

	public UnityEngine.UI.Image topL;
	public UnityEngine.UI.Image topR;
	public UnityEngine.UI.Image bottomL;
	public UnityEngine.UI.Image bottomR;
	public UnityEngine.UI.Image topBG;
	public UnityEngine.UI.Image bottomBG;

	private DashBoost dashBoost;
	private TeleBoost teleBoost;

	private float value;

	private float red = 0f;
	private float alpha = 0.5f;
	private float fadeDelay = 0.4f; 

	// Use this for initialization
	void Start () {
		if (GameObject.Find("DashBoost")) {
			dashBoost = GameObject.Find("DashBoost").GetComponent<DashBoost>();
		}
		if (GameObject.Find("TeleBoost")) {
			teleBoost = GameObject.Find("TeleBoost").GetComponent<TeleBoost>();
		}

		value = dashBoost.charges / 4f;

		topBG.color = new Color(0.5f, 0.5f, 0.5f, 0f);
		bottomBG.color = new Color(0.5f, 0.5f, 0.5f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		float fill = 0f;
		if (dashBoost) fill = dashBoost.charges / 4f;
		if (teleBoost) fill = teleBoost.charges / 4f;
		value = Mathf.Lerp(value, fill, 0.2f);
		topL.fillAmount = value;
		topR.fillAmount = value;
		bottomL.fillAmount = value;
		bottomR.fillAmount = value;

		Color color;
		// non-full dash
		if (fill < 1f) {
			alpha = 0.5f;
			fadeDelay = 0.4f;

			if (fill < 0.25f) {
				if (dashBoost) red = 0.2f;
				if (teleBoost) red = 0.2f;
				color = new Color (1f, red, red, alpha);
			}
			else {
				red = 1f;
			}
			color = new Color (1f, red, red, alpha);
		}
		// full dash
		else {
			red = 1f;
			fadeDelay -= Time.deltaTime;
			if (fadeDelay < 0f && alpha > 0f) {
				alpha -= Time.deltaTime;
			}
		}
		color = new Color(1f, red, red, alpha);

		topL.color = color;
		topR.color = color;
		bottomL.color = color;
		bottomR.color = color;

		topBG.color = new Color(0.5f, 0.5f, 0.5f, alpha * 0.2f);
		bottomBG.color = new Color(0.5f, 0.5f, 0.5f, alpha * 0.2f);
	}
}
