using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {

	public Cinemachine.CinemachineVirtualCamera startCutscene;
	public Cinemachine.CinemachineVirtualCamera startCutscene2;

	// Use this for initialization
	void Start () {
		startCutscene = GameObject.Find("BeginningCutsceneCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
		startCutscene2 = GameObject.Find("BeginningCutsceneCam2").GetComponent<Cinemachine.CinemachineVirtualCamera>();
		StartCoroutine(StartCutscene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator StartCutscene() {
		startCutscene.Priority = 15;
		yield return new WaitForSeconds(0.8f);
		startCutscene.Priority = 5;
		startCutscene2.Priority = 15;
		yield return new WaitForSeconds(3.5f);
		startCutscene2.Priority = 5;
	}
}
