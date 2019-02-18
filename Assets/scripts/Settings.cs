using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Settings.LoadData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void SaveData()
    {
        for (int i = 1; i <= 12; i++)
        {
            PlayerPrefs.SetInt("LevelClear" + i, 0);
            PlayerPrefs.SetInt("FreeModeUnlocked", 0);
        }
    }

    public static void ChangeGameModifiers(int mode)
    {
        PlayerPrefs.SetInt("Gamemode", 0);
        PlayerPrefs.SetInt("Wavemode", 0);
    }

    public static void AddToScore(int score)
    {
        PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + score);
    }

    public static void LoadData()
    {

    }
}