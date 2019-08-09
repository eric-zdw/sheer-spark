using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPManager : MonoBehaviour {

    UnityEngine.PostProcessing.PostProcessingProfile ppProfile;
    UnityEngine.PostProcessing.ColorGradingModel.Settings ppSettings;
	public float ppDefaultContrast = 1f;
    public float ppSlowContrast = 0.25f;
    public float ppDefaultSaturation = 1f;
    public float ppSlowSaturation = -0.25f;
    public float ppDefaultExposure = 0f;
    public float ppSlowExposure = 0.5f;
    public float ppTransitionTime = 2.5f;
    public float ppDefaultTimeScale = 1.1f;
    public float ppSlowTimeScale = 0.011f;
    public float ppFixedScale = 60f;
	public float ppTimeIncreaseRate = 1.01f;

	// Use this for initialization
	void Start () {
		ppProfile = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile;
        ppSettings = ppProfile.colorGrading.settings;
        ppSettings.basic.contrast = ppDefaultContrast;
        ppSettings.basic.saturation = ppDefaultSaturation;
        ppSettings.basic.postExposure = ppDefaultExposure;
        ppProfile.colorGrading.settings = ppSettings;

        Time.timeScale = ppDefaultTimeScale;
        Time.fixedDeltaTime = ppDefaultTimeScale / ppFixedScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator SlowDown() {
		float ppTimer = ppTransitionTime;
		float ppMagnitude = ppTimer / ppTransitionTime;

		while (ppTimer > 0f) {
			if (!WaveSystem.isPaused) {
				ppTimer -= 0.01f;
				ppMagnitude = ppTimer / ppTransitionTime;

				ppSettings.basic.contrast = ppDefaultContrast + (ppSlowContrast * ppMagnitude);
				ppSettings.basic.saturation = ppDefaultSaturation + (ppSlowSaturation * ppMagnitude);
				ppSettings.basic.postExposure = ppDefaultExposure + (ppSlowExposure * ppMagnitude);
				ppProfile.colorGrading.settings = ppSettings;
			}
			yield return new WaitForSecondsRealtime(0.01f);
		}

		ppSettings.basic.contrast = ppDefaultContrast;
        ppSettings.basic.saturation = ppDefaultSaturation;
        ppSettings.basic.postExposure = ppDefaultExposure;
        ppProfile.colorGrading.settings = ppSettings;
    }

	public IEnumerator ChangePP() {
		Time.timeScale = ppSlowTimeScale;
		Time.fixedDeltaTime = Time.timeScale / 60f;

		while (Time.timeScale < ppDefaultTimeScale) {
			if (!WaveSystem.isPaused) {
				Time.timeScale *= ppTimeIncreaseRate;
				Time.fixedDeltaTime = Time.timeScale / 60f;

				print(Time.timeScale);

				if (Time.timeScale >= ppDefaultTimeScale) {
					Time.timeScale = ppDefaultTimeScale;
				}
			}
			yield return new WaitForSecondsRealtime(0.01f);
		}
		
	}

	public IEnumerator HitStun() {
		Time.timeScale = 0.11f;
		bool paused = false;
		print(Time.timeScale);
		while (paused == false) {
			yield return new WaitForEndOfFrame();
			paused = true;
		}
		
		Time.timeScale = 1.1f;
		print(Time.timeScale);
	}

	public IEnumerator GameEndEffects() {
		ppSettings.basic.contrast = ppDefaultContrast + ppSlowContrast;
		ppSettings.basic.saturation = ppDefaultSaturation + ppSlowSaturation;
		ppSettings.basic.postExposure = ppDefaultExposure + 10f;
		ppProfile.colorGrading.settings = ppSettings;

		Time.timeScale = 0.022f;
		Time.fixedDeltaTime = Time.timeScale / 60f;

		while (true) {
			ppSettings.basic.postExposure -= 2f * Time.unscaledDeltaTime;
			ppProfile.colorGrading.settings = ppSettings;

			yield return new WaitForEndOfFrame();
		}
	}
}
