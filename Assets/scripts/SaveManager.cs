using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum WeaponColor {Red, Orange, Yellow, Green, Blue, Purple};

[System.Serializable]
public class SaveManager : MonoBehaviour {

    public static SaveData saveData = new SaveData();
    string savePath;

	// Use this for initialization
	void Start () {
        SaveManager.LoadSaveData(saveData);

        //print("SaveManager: levelsClearedOnNormal: ");
        foreach (string level in saveData.levelsClearedOnNormal) {
            //print(level);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    public static void WriteToFile(SaveData data)
    {
        string newJson = JsonUtility.ToJson(data);

        using (StreamWriter writer = File.CreateText(Application.persistentDataPath + "/savedata.json")) {
            writer.Write(newJson);
        }

        print("file saved");
    }

    public static void LoadSaveData(SaveData data)
    {
        if (File.Exists(Application.persistentDataPath + "/savedata.json")) {
            string newJson = File.ReadAllText(Application.persistentDataPath + "/savedata.json");
            JsonUtility.FromJsonOverwrite(newJson, data);

            print("file loaded");
        }
    }

    public int[] GetSelectedWeapons(SaveData data) {
        int[] weaponsList = new int[6];
        weaponsList[0] = PlayerPrefs.GetInt("SelectedWeaponRed", 0);
        weaponsList[1] = PlayerPrefs.GetInt("SelectedWeaponOrange", 0);
        weaponsList[2] = PlayerPrefs.GetInt("SelectedWeaponYellow", 0);
        weaponsList[3] = PlayerPrefs.GetInt("SelectedWeaponGreen", 0);
        weaponsList[4] = PlayerPrefs.GetInt("SelectedWeaponBlue", 0);
        weaponsList[5] = PlayerPrefs.GetInt("SelectedWeaponPurple", 0);

        return weaponsList;
    }

    public static int GetSelectedWeapon(WeaponColor color)
    {
        switch (color)
        {
            case WeaponColor.Red:
                return PlayerPrefs.GetInt("SelectedWeaponRed", 0);
            case WeaponColor.Orange:
                return PlayerPrefs.GetInt("SelectedWeaponOrange", 0);
            case WeaponColor.Yellow:
                return PlayerPrefs.GetInt("SelectedWeaponYellow", 0);
            case WeaponColor.Green:
                return PlayerPrefs.GetInt("SelectedWeaponGreen", 0);
            case WeaponColor.Blue:
                return PlayerPrefs.GetInt("SelectedWeaponBlue", 0);
            case WeaponColor.Purple:
                return PlayerPrefs.GetInt("SelectedWeaponPurple", 0);
        }

        return 0;
    }

    public static void SetSelectedWeapon(WeaponColor color, int selection) {
        switch (color) {
            case WeaponColor.Red:
                PlayerPrefs.SetInt("SelectedWeaponRed", selection);
                break;
            case WeaponColor.Orange:
                PlayerPrefs.SetInt("SelectedWeaponOrange", selection);
                break;
            case WeaponColor.Yellow:
                PlayerPrefs.SetInt("SelectedWeaponYellow", selection);
                break;
            case WeaponColor.Green:
                PlayerPrefs.SetInt("SelectedWeaponGreen", selection);
                break;
            case WeaponColor.Blue:
                PlayerPrefs.SetInt("SelectedWeaponBlue", selection);
                break;
            case WeaponColor.Purple:
                PlayerPrefs.SetInt("SelectedWeaponPurple", selection);
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}