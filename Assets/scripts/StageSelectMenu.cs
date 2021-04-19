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

	public GameObject contentList;
	public GameObject stageSelectEntryPrefab;
	public StagePreviewCanvas stagePreview;
	public GameObject stagePreviewPanel;

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
		foreach (StageEntry addse in availableStages)
        {
			GameObject stageSelectEntry = Instantiate(stageSelectEntryPrefab);
			stageSelectEntry.transform.SetParent(contentList.transform);
			stageSelectEntry.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = addse.StageName;
			stageSelectEntry.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { JumpToStage(addse.StageName); });
		}
	}

	void JumpToStage(string name)
    {
		stagePreview.LoadStagePreviewAction(name);
		stagePreviewPanel.SetActive(true);
		gameObject.SetActive(false);
	}
}
