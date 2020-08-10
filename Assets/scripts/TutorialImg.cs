using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialImg : MonoBehaviour {

    private TutorialTrigger tt;
    public UnityEngine.UI.Image im;

	// Use this for initialization
	void Start () {
        tt = GameObject.FindGameObjectWithTag("TutorialTrigger").GetComponent<TutorialTrigger>();
	}
	
	// Update is called once per frame
	void Update () {
        if (tt) {
            if (tt.isHelping == false && im.enabled == true)
                im.enabled = false;
            else if (tt.isHelping == true && im.enabled == false)
                im.enabled = true;
            else if (tt == null)
                Destroy(gameObject);
        }
        else {
            Destroy(gameObject);
        }

    }
}
