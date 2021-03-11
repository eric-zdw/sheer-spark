using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public GameObject gameUI;
	public GameObject fadeObject;

	// Use this for initialization
	void Start () {
		gameUI = GameObject.Find("GameUI");
		fadeObject = Instantiate(fadeObject, gameUI.transform);
		fadeObject.GetComponent<Fade>().StartCoroutine("FadeIn");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
