using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectMenu : MonoBehaviour {

	public List<string> levels = new List<string>();
	public UnityEngine.UI.Text currentLevelName;
	private GameObject currentLevelModel;
	private int currentLevel = 0;
	public GameObject leftButton;
	public GameObject rightButton;
	private Camera cam;

	private SaveData saveData;

	public List<int> levelIndices = new List<int>();
	public List<GameObject> levelModels = new List<GameObject>();
	public List<Vector3> levelPositions = new List<Vector3>();
	public List<Color> bgColours = new List<Color>();

	// Use this for initialization
	void Start () {
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		saveData = GameObject.Find("SaveManager").GetComponent<SaveManager>().saveData;
	}

	// Update is called once per frame
	void Update () {

	}

	public void StartUp() {
		currentLevelName.text = levels[currentLevel];
		currentLevelModel = Instantiate(levelModels[currentLevel], levelPositions[currentLevel], Quaternion.identity);
	}

	void IncrementLevel() {
		currentLevel++;
		currentLevelName.text = levels[currentLevel];
		
		Destroy(currentLevelModel);
		currentLevelModel = Instantiate(levelModels[currentLevel], levelPositions[currentLevel], Quaternion.identity);

		
		leftButton.SetActive(true);
		//check if at the end of the list and level is complete
		if (saveData.levelsClearedOnNormal.Contains(levels[currentLevel]) && currentLevel != (levels.Count - 1)) {
			rightButton.SetActive(true);
		}
		else {
			rightButton.SetActive(false);
		}
	}

	void DecrementLevel() {
		currentLevel--;
		currentLevelName.text = levels[currentLevel];
		
		Destroy(currentLevelModel);
		currentLevelModel = Instantiate(levelModels[currentLevel], levelPositions[currentLevel], Quaternion.identity);

		rightButton.SetActive(true);
		//check if at the end of the level list
		if (currentLevel != 0) {
			leftButton.SetActive(true);
		}
		else {
			leftButton.SetActive(false);
		}
	}

	void CheckIfCompleted() {

	}

	void OpenOptions() {

	}

	public void Cleanup() {
		Destroy(currentLevelModel);
	}
}
