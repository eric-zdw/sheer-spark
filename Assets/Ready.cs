using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ready : MonoBehaviour {

    float routineTimer = 15f;
    UnityEngine.UI.Text text;

    bool checkpoint1 = false;
    bool checkpoint2 = false;
    bool checkpoint3 = false;
    bool checkpoint4 = false;
    bool checkpoint5 = false;

    // Use this for initialization
    void Start () {
        text = GetComponent<UnityEngine.UI.Text>();
        text.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        routineTimer -= Time.deltaTime;

        if (routineTimer < 13f && checkpoint1 == false)
        {
            checkpoint1 = true;
            text.fontSize = 250;
            text.text = "GET READY";
            text.enabled = true;
        }

        if (routineTimer < 12.5f && checkpoint2 == false)
        {
            if (text.color.a > 0)
                text.color = new Color(text.color.r, text.color.b, text.color.g, text.color.a - 5f * Time.deltaTime);
            else
                checkpoint2 = true;
        }

        if (routineTimer < 10f && checkpoint3 == false)
        {
            text.fontSize = 800;
            text.color = new Color(255, 255, 255, 0.5f);
            text.text = Mathf.Ceil(routineTimer).ToString();

            if (routineTimer < 1f)
                checkpoint3 = true;
        }

        if (routineTimer < 0f && checkpoint4 == false)
        {
            text.text = "START";
            checkpoint4 = true;
        }

        if (routineTimer < -0.5f && checkpoint5 == false)
        {
            if (text.color.a > 0)
                text.color = new Color(text.color.r, text.color.b, text.color.g, text.color.a - 2f * Time.deltaTime);
            else
                checkpoint5 = true;
        }

    }
}
