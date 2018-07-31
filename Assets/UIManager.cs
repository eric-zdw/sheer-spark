using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public UnityEngine.UI.Image fade;

	// Use this for initialization
	void Start () {
		fade.GetComponent<Fade>().StartCoroutine("FadeIn");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
