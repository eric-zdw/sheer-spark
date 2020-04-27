using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public static int score = 0;
	public static float multiplier = 1;
	
	private static float multiplierDecayRate = 0.2f;
	private PlayerBehaviour player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		
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
