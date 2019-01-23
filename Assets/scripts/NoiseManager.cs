using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour {

	private Cinemachine.CinemachineBasicMultiChannelPerlin noiseSettings;
	private float amplitude = 0f;

	// Use this for initialization
	void Start () {
		noiseSettings = GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin> ();
		StartCoroutine(manageNoise());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddNoise(float a) {
		amplitude += a;
	}

	IEnumerator manageNoise() {
		while (true) {
			amplitude *= 0.5f;
			noiseSettings.m_AmplitudeGain = amplitude;
			yield return new WaitForSeconds(0.1f);
		}
	}
}
