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
}