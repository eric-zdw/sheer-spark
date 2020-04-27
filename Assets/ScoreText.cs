using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour {
	public ScoreManager scoreManager;
	private float scoreDisplay = 1f;

	private UnityEngine.UI.Text scoreText;
	private UnityEngine.UI.Text multiplierText;


	// Use this for initialization
	void Start () {
		scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

		scoreText = transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
		multiplierText = transform.GetChild(1).GetComponent<UnityEngine.UI.Text>();

		StartCoroutine(UpdateScore());
	}

	private IEnumerator UpdateScore() {
		while (true) {
			scoreDisplay = Mathf.Lerp(scoreDisplay, ScoreManager.score, 0.1f);
			scoreText.text = Mathf.RoundToInt(scoreDisplay).ToString("D8");
			multiplierText.text = "x" + ((Mathf.Round(ScoreManager.multiplier * 10f)) / 10f).ToString();

			yield return new WaitForEndOfFrame();
		}
	}
}
