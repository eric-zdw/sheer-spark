using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public static int score = 0;
	public static float multiplier = 1;
	
	private static float multiplierDecayRate = 2f;
	private PlayerBehaviour player;
	private float multiplierVelocity = 0f;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

		StartCoroutine(MultiplierDecay());
	}

	private IEnumerator MultiplierDecay() {
		while (true) {
			if (multiplier > 1f) {
				//multiplier = ((multiplier - 1f) * 0.999f) + 1f;
				multiplier = Mathf.SmoothDamp(multiplier, 1f, ref multiplierVelocity, 60f);
			}
			if (multiplier < 1f) {
				multiplier = 1f;
			}

			yield return new WaitForSeconds(0.02f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void ResetScore() {
		score = 0;
		multiplier = 1f;
	}

	public static void IncreaseScore(int value) {
		//round multiplier to one digit
		float roundedMultiplier = Mathf.Round(multiplier * 10f) / 10f;
		score += (int)(value * roundedMultiplier);
	}

	public static void IncreaseMultiplier(float value) {
		multiplier += value;
	}
}
