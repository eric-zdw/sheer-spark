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

	public float modelDistance = 100f;
	public float rotateSpeed = 50f;

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
		currentLevelModel.transform.RotateAround(currentLevelModel.transform.position, transform.up, rotateSpeed * Time.deltaTime);
	}

	public void StartUp() {
		currentLevelName.text = levels[currentLevel];
		Vector3 modelPosition = (Camera.main.transform.position + Camera.main.transform.forward * modelDistance);
		currentLevelModel = Instantiate(levelModels[currentLevel], modelPosition, Camera.main.transform.rotation);
	}

	void IncrementLevel() {
		currentLevel++;
		currentLevelName.text = levels[currentLevel];
		Vector3 modelPosition = (Camera.main.transform.position + Camera.main.transform.forward * modelDistance);
		
		Destroy(currentLevelModel);
		currentLevelModel = Instantiate(levelModels[currentLevel], modelPosition, Camera.main.transform.rotation);

		
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
		Vector3 modelPosition = (Camera.main.transform.position + Camera.main.transform.forward * modelDistance);
		
		Destroy(currentLevelModel);
		currentLevelModel = Instantiate(levelModels[currentLevel], modelPosition, Camera.main.transform.rotation);

		rightButton.SetActive(true);
		//check if at the end of the level list
		if (currentLevel != 0) {
			leftButton.SetActive(true);
		}
		else {
			leftButton.SetActive(false);
		}
	}

	public void StartGame() {
		UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndices[currentLevel]);
	}

	void CheckIfCompleted() {

	}

	void OpenOptions() {

	}

	public void Cleanup() {
		Destroy(currentLevelModel);
	}
}
