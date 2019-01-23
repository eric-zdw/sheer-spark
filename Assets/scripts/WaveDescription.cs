using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDescription : MonoBehaviour {

    public bool waveStarted = false;
    private float waveTimer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (waveStarted)
        {
            waveTimer -= Time.deltaTime;
        }
	}

    public void startWave()
    {
        waveStarted = true;
        waveTimer = 15f;
    }
}
