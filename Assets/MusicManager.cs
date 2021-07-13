using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    AudioSource lastMusic;
    AudioSource newMusic;

    public AudioClip[] musics;
    private AudioSource[] sources;
    private bool[] activeMusics;
    private bool musicChosen = false;

	// Use this for initialization
	void Start () {
        sources = GetComponents<AudioSource>();
        for (int i = 0; i < 6; i++) {
            sources[i].clip = musics[i];
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 1; i < 6; i++)
        {
            if (sources[i].isPlaying)
            {
                sources[i].timeSamples = sources[0].timeSamples;
            }
        }
    }

    public void StartMusic() {
        for (int i = 0; i < 6; i++) {
            sources[i].Play();
        }
    }

    public IEnumerator ChangeMusic(int index) {
        float volume = 0f;
        while (volume < 1f) {
            //print("music changing");
            volume += Time.unscaledDeltaTime * 0.2f;
            sources[index].volume = volume;

            for (int i = 0; i < 6; i++) {
                if (i != index) {
                    sources[i].volume -= Time.unscaledDeltaTime * 0.2f;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
