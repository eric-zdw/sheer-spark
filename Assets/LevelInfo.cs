using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour {
	public string levelName;
	public int sceneIndex;

	public LevelInfo(string name) {
		levelName = name;
		sceneIndex = 0;
	}
}
