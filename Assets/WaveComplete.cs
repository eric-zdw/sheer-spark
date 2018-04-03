using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveComplete : MonoBehaviour {

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

        if (timer <= 15f && timer >= 12f)
        {
            if (txt.enabled == false)
                txt.enabled = true;

            rt.anchorMin = new Vector2(0.1f, 0.1f);
            rt.anchorMax = new Vector2(0.9f, 0.9f);
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;

            txt.text = "WAVE " + (ws.waveNumber - 1) + " COMPLETE";
        }

        if (timer <= 12f && timer >= 10f)
        {
            if (txt.enabled == true)
                txt.enabled = false;
        }

        if (timer <= 11f && timer > 0f)
        {
            if (txt.enabled == false)
                txt.enabled = true;

            rt.anchorMin = new Vector2(0.3f, 0.6f);
            rt.anchorMax = new Vector2(0.7f, 0.8f);
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;

            txt.text = "NEXT WAVE STARTS IN";
        }
	}

    public void StartRoutine()
    {
        timer = 15f;
    }
}
