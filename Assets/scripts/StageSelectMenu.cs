using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectMenu : MonoBehaviour {
	public StageEntry[] stageEntries;
	private List<StageEntry> availableStages;
	private int stageSelected;

	private UnityEngine.UI.Text nameText;
	private UnityEngine.UI.Button leftButton;
	private UnityEngine.UI.Button rightButton;

	void Start() {
		availableStages = new List<StageEntry>();
		foreach (StageEntry se in stageEntries) {
			print("Stage Entry: " + se.StageName);
			bool add = true;
			foreach (string stages in se.Prerequisites) {
				//do not add the stage to the menu if prerequisite stages are not completed.
				if (!SaveManager.saveData.levelsClearedOnNormal.Contains(stages)) {
					add = false;
				}
			}
			if (add) {
				print("Added.");
				availableStages.Add(se);
			}
		}

		nameText = GameObject.Find("StageName").GetComponent<UnityEngine.UI.Text>();
		leftButton = GameObject.Find("LeftButton").GetComponent<UnityEngine.UI.Button>();
		rightButton = GameObject.Find("RightButton").GetComponent<UnityEngine.UI.Button>();

		stageSelected = 0;
		UpdateMenu();
	}

	public void NextStage() {
		stageSelected++;
		UpdateMenu();
	}

	public void PreviousStage() {
		stageSelected--;
		UpdateMenu();
	}

	public void BackToMenu() {

	}

	private void UpdateMenu() {
		nameText.text = availableStages[stageSelected].StageName;
		PlayerPrefs.SetInt("stageSelected", stageSelected);

		if (stageSelected == availableStages.Count - 1) {
			rightButton.enabled = false;
		}
		else {
			rightButton.enabled = true;
		}

		if (stageSelected == 0) {
			leftButton.enabled = false;
		}
		else {
			leftButton.enabled = true;
		}
	}

	public void Play() {
		SceneManager.LoadScene(availableStages[stageSelected].SceneIndex);
	}
}
