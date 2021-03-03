using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipText : MonoBehaviour {

	public List<string> tipList = new List<string>();
	public Text tipText;

	// Use this for initialization
	void Start () {
		int selection = Random.Range(0, tipList.Count);
		tipText.text = tipList[selection].ToUpper();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
