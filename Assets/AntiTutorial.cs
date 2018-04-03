using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiTutorial : MonoBehaviour {

    private TutorialTrigger tt;
    public UnityEngine.UI.Text text;

	// Use this for initialization
	void Start () {
        tt = GameObject.FindGameObjectWithTag("TutorialTrigger").GetComponent<TutorialTrigger>();
	}
	
	// Update is called once per frame
	void Update () {
        if (tt.isHelping == false && text.enabled == false)
            text.enabled = true;
        else if (tt.isHelping == true && text.enabled == true)
            text.enabled = false;
        else if (tt == null)
            Destroy(gameObject);
    }
}
