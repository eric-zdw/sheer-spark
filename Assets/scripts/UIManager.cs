using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	private GameObject ui;
	public GameObject fadeObject;

	// Use this for initialization
	void Start () {
		ui = GameObject.Find("UI");
		fadeObject = Instantiate(fadeObject, ui.transform);
		fadeObject.GetComponent<Fade>().StartCoroutine("FadeIn");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
