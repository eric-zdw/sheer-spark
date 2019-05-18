using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelList : MonoBehaviour {

	public List<LevelInfo> levels;

	public LevelList() {
		levels = new List<LevelInfo>();

		levels.Add(new LevelInfo("test"));
	}

}
