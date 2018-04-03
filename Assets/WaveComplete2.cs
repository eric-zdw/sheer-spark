using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveComplete2 : MonoBehaviour {

    private float timer = 0f;
    private UnityEngine.UI.Text txt;
    private WaveSystem ws;
    private RectTransform rt;

	// Use this for initialization
	void Start () {
        txt = GetComponent<UnityEngine.UI.Text>();
        rt = GetComponent<RectTransform>();
        ws = GameObject.FindGameObjectWithTag("WaveSystem").GetComponent<WaveSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
            txt.enabled = false;

        if (timer <= 10f && timer > 0f)
        {
            if (txt.enabled == false)
                txt.enabled = true;

            txt.color = new Color(1f, 1f, 1f, 0.3f);
            txt.text = Mathf.Ceil(timer).ToString();
        }
	}

    public void StartRoutine()
    {
        timer = 15f;
    }
}
