using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    AudioSource lastMusic;
    AudioSource newMusic;

    public AudioSource[] musics;
    private bool[] activeMusics;
    private bool musicChosen = false;

	// Use this for initialization
	void Start () {

		musics = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
        lastMusic = musics[0];
        newMusic = musics[0];
        activeMusics = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            activeMusics[i] = false;
        }
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
