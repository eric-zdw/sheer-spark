using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectMenu : MonoBehaviour {

	public List<string> levels = new List<string>();
	public UnityEngine.UI.Text stageName;
	private int currentLevel = 0;
	public GameObject leftButton;
	public GameObject rightButton;

	// Use this for initialization
	void Start () {
		stageName.text = levels[currentLevel];
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
		stageName.text = levels[currentLevel];
	}

	void DecrementLevel() {
		currentLevel--;
		stageName.text = levels[currentLevel];
	}
}
