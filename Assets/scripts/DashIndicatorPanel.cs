using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIndicatorPanel : MonoBehaviour {

	public UnityEngine.UI.Image topL;
	public UnityEngine.UI.Image topR;
	public UnityEngine.UI.Image bottomL;
	public UnityEngine.UI.Image bottomR;

	private DashBoost dashBoost;

	// Use this for initialization
	void Start () {
		dashBoost = GameObject.Find("DashBoost").GetComponent<DashBoost>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float fill = dashBoost.charges / 4f;
		topL.fillAmount = fill;
		topR.fillAmount = fill;
		bottomL.fillAmount = fill;
		bottomR.fillAmount = fill;

		Color color;
		if (fill < 0.9f) {
			float alpha = 0.9f - (dashBoost.charges / 3.6f);
			float red = 0f;
			if (fill < 0.5f) {
				red = dashBoost.charges / 2f;
				//color = new Color (1f, 1f - red, 1f - red, alpha);
			}
			else {
				red = 1f;
			}
			color = new Color (1f, red, red, alpha);
		}
		else {
			color = new Color(1f, 1f, 1f, 0f);
		}

		topL.color = color;
		topR.color = color;
		bottomL.color = color;
		bottomR.color = color;
	}
}
