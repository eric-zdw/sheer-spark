using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public static int score = 0;
	public static float multiplier = 1;
	public static float exponentPart = 500f;
	const float basePart = 1f;
	
	private static float multiplierDecayRate = 2f;
	private PlayerBehaviour player;
	private float multiplierVelocity = 0f;
	const float updateInterval = 0.02f;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

		StartCoroutine(MultiplierDecay());
	}

	private IEnumerator MultiplierDecay() {

		float timer = updateInterval;

		while (true) {
			timer -= Time.deltaTime;
			// in the event of Time.deltaTime taking too much time, multipler should decay several intervals
			while (timer <= 0f) {
				if (exponentPart > 0f) {
					exponentPart -= 0.004f;
					exponentPart *= 0.9999f;
					multiplier = basePart + exponentPart;
				}
				else if (exponentPart < 0f) {
					exponentPart = 0f;
				}
				timer += updateInterval;
			}
			
			yield return new WaitForFixedUpdate();
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
		exponentPart += value;
	}
}
