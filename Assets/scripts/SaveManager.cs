using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public SaveData saveData = new SaveData();
    string savePath;

	// Use this for initialization
	void Start () {
        savePath = Path.Combine(Application.persistentDataPath, "SaveData.txt");
        SaveManager.WriteToFile(saveData, savePath);
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    public static void WriteToFile(SaveData data, string path)
    {
        string newJson = JsonUtility.ToJson(data);

        using (StreamWriter writer = File.CreateText(path)) {
            writer.Write(newJson);
        }

        print("file saved at: " + path);
    }

    public int[] GetSelectedWeapons() {
        int[] weaponsList = new int[6];
        weaponsList[0] = PlayerPrefs.GetInt("SelectedWeaponRed", 1);
        weaponsList[1] = PlayerPrefs.GetInt("SelectedWeaponOrange", 1);
        weaponsList[2] = PlayerPrefs.GetInt("SelectedWeaponYellow", 1);
        weaponsList[3] = PlayerPrefs.GetInt("SelectedWeaponGreen", 1);
        weaponsList[4] = PlayerPrefs.GetInt("SelectedWeaponBlue", 1);
        weaponsList[5] = PlayerPrefs.GetInt("SelectedWeaponPurple", 1);

        return weaponsList;
    }

    public void SetSelectedWeapon(string color, int type) {
        switch (color) {
            case "red":
                PlayerPrefs.SetInt("SelectedWeaponRed", type);
                break;
            case "orange":
                PlayerPrefs.SetInt("SelectedWeaponOrange", type);
                break;
            case "yellow":
                PlayerPrefs.SetInt("SelectedWeaponYellow", type);
                break;
            case "green":
                PlayerPrefs.SetInt("SelectedWeaponGreen", type);
                break;
            case "blue":
                PlayerPrefs.SetInt("SelectedWeaponBlue", type);
                break;
            case "purple":
                PlayerPrefs.SetInt("SelectedWeaponPurple", type);
                break;
        }
    }
}