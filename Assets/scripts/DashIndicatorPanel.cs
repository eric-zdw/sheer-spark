using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIndicatorPanel : MonoBehaviour {

	public UnityEngine.UI.Image topL;
	public UnityEngine.UI.Image topR;
	public UnityEngine.UI.Image bottomL;
	public UnityEngine.UI.Image bottomR;
	public UnityEngine.UI.Image topL2;
	public UnityEngine.UI.Image topR2;
	public UnityEngine.UI.Image bottomL2;
	public UnityEngine.UI.Image bottomR2;
	public UnityEngine.UI.Image topBG;
	public UnityEngine.UI.Image bottomBG;

	private DashBoost dashBoost;
	private TeleBoost teleBoost;

	private float valueWidth, valueHeight;
	private float percent;

	private float red = 0f;
	private float alpha = 0.5f;
	private float fadeDelay = 0.4f; 

	private float totalPixels;

	// Use this for initialization
	void Start () {
		if (GameObject.Find("DashBoost")) {
			dashBoost = GameObject.Find("DashBoost").GetComponent<DashBoost>();
		}
		if (GameObject.Find("TeleBoost")) {
			teleBoost = GameObject.Find("TeleBoost").GetComponent<TeleBoost>();
		}

		percent = dashBoost.uses / 4f;

		topBG.color = new Color(0.5f, 0.5f, 0.5f, 0f);
		bottomBG.color = new Color(0.5f, 0.5f, 0.5f, 0f);

		int barWidth = Screen.height / 100;
		topL.rectTransform.sizeDelta = new Vector2(topL.rectTransform.sizeDelta.x, barWidth);
		topR.rectTransform.sizeDelta = new Vector2(topR.rectTransform.sizeDelta.x, barWidth);
		bottomL.rectTransform.sizeDelta = new Vector2(bottomL.rectTransform.sizeDelta.x, barWidth);
		bottomR.rectTransform.sizeDelta = new Vector2(bottomR.rectTransform.sizeDelta.x, barWidth);
		topBG.rectTransform.sizeDelta = new Vector2(topBG.rectTransform.sizeDelta.x, barWidth);
		bottomBG.rectTransform.sizeDelta = new Vector2(bottomBG.rectTransform.sizeDelta.x, barWidth);

		bottomL2.rectTransform.sizeDelta = new Vector2(barWidth, Screen.height / 2);
		bottomR2.rectTransform.sizeDelta = new Vector2(barWidth, Screen.height / 2);
		topL2.rectTransform.sizeDelta = new Vector2(barWidth, Screen.height / 2);
		topR2.rectTransform.sizeDelta = new Vector2(barWidth, Screen.height / 2);


	}
	
	// Update is called once per frame
	void Update () {
		totalPixels = Screen.width + Screen.height;
		float heightRatio = Screen.height / totalPixels;
		float widthRatio = Screen.width / totalPixels;
		
		percent = dashBoost.uses / 4f;
		float fillHeight = (1f - widthRatio - (1f - percent)) / heightRatio;
		float fillWidth = (1f - (1f - percent)) / widthRatio;
		
		//if (dashBoost) fill = dashBoost.uses / 4f;
		//if (teleBoost) fill = teleBoost.uses / 4f;
		valueHeight = Mathf.Clamp(Mathf.Lerp(valueHeight, fillHeight, 0.25f), 0f, 1f);
		valueWidth = Mathf.Clamp(Mathf.Lerp(valueWidth, fillWidth, 0.25f), 0f, 2f);

		topL.fillAmount = valueWidth;
		topR.fillAmount = valueWidth;
		bottomL.fillAmount = valueWidth;
		bottomR.fillAmount = valueWidth;
		topL2.fillAmount = valueHeight;
		topR2.fillAmount = valueHeight;
		bottomL2.fillAmount = valueHeight;
		bottomR2.fillAmount = valueHeight;

		Color color;
		// non-full dash
		if (percent < 1f) {
			alpha = 0.5f;
			fadeDelay = 0.4f;

			if (percent < 0.25f) {
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
		topL2.color = color;
		topR2.color = color;
		bottomL2.color = color;
		bottomR2.color = color;

		topBG.color = new Color(0.5f, 0.5f, 0.5f, alpha * 0.2f);
		bottomBG.color = new Color(0.5f, 0.5f, 0.5f, alpha * 0.2f);
	}
}
