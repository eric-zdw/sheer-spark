using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectMenu : MonoBehaviour {

	public List<string> levels = new List<string>();
	public UnityEngine.UI.Text currentLevelName;
	private GameObject currentLevelModel;
	private int currentLevel = 0;
	public GameObject leftButton;
	public GameObject rightButton;

	public List<GameObject> levelModels = new List<GameObject>();
	public List<Vector3> levelPositions = new List<Vector3>();

	// Use this for initialization
	void Start () {
		currentLevelName.text = levels[currentLevel];
		currentLevelModel = Instantiate(levelModels[currentLevel], levelPositions[currentLevel], Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
		if (currentLevel == 0 && leftButton.activeSelf == true)
			leftButton.SetActive (false);
		else if (currentLevel != 0 && leftButton.activeSelf == false)
			leftButton.SetActive (true);

		if (currentLevel == levels.Count - 1 && rightButton.activeSelf == true)
			rightButton.SetActive (false);
		else if (currentLevel != levels.Count - 1 && rightButton.activeSelf == false)
			rightButton.SetActive (true);
	}

	void IncrementLevel() {
		currentLevel++;
		currentLevelName.text = levels[currentLevel];
		SwitchLevelModel(currentLevel);
	}

	void DecrementLevel() {
		currentLevel--;
		currentLevelName.text = levels[currentLevel];
		SwitchLevelModel(currentLevel);
	}

	void SwitchLevelModel(int currentLevel) {
		Destroy(currentLevelModel);
		currentLevelModel = Instantiate(levelModels[currentLevel], levelPositions[currentLevel], Quaternion.identity);
	}

	void OpenOptions() {

	}

	void Cleanup() {
		Destroy(currentLevelModel);
	}
}
