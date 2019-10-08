using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuData : MonoBehaviour {

	static int[] weaponsSelected = {0, 0, 0, 0, 0, 0};

	public enum GameMode {Main, Free};
	static GameMode modeSelected = GameMode.Main;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void SetGameMode(GameMode gm) {
		modeSelected = gm;
	}
}
