using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {

	public List<Cinemachine.CinemachineVirtualCamera> cutsceneCams;
	public List<float> cutsceneTimes;

	// Use this for initialization
	void Start () {
		StartCoroutine(StartCutscene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator StartCutscene() {
		for (int i = 0; i < cutsceneCams.Count; i++) {
			cutsceneCams[i].Priority = 15;
			yield return new WaitForSeconds(cutsceneTimes[i]);
			cutsceneCams[i].Priority = 5;
		}
	}
}
