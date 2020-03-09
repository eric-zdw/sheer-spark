using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuData : MonoBehaviour {

	public SaveData saveData = new SaveData();
    string savePath;

	static int[] weaponsSelected = {0, 0, 0, 0, 0, 0};

	public enum GameMode {Main, Free};
	static GameMode modeSelected = GameMode.Main;

	// Use this for initialization
	void Start () {
		//load from save data

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMainMode() {
		modeSelected = GameMode.Main;
	}

	public void SetFreeMode() {
		modeSelected = GameMode.Free;
	}
}
