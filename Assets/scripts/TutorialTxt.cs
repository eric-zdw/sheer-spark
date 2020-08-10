using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTxt : MonoBehaviour {

    private TutorialTrigger tt;
    public UnityEngine.UI.Text txt;

	// Use this for initialization
	void Start () {
        tt = GameObject.FindGameObjectWithTag("TutorialTrigger").GetComponent<TutorialTrigger>();
	}
	
	// Update is called once per frame
	void Update () {
        if (tt) {
            if (tt.isHelping == false && txt.enabled == true)
                txt.enabled = false;
            else if (tt.isHelping == true && txt.enabled == false)
                txt.enabled = true;
            else if (tt == null)
                Destroy(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        
	}
}
