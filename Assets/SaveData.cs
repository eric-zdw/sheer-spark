using System;
using System.Collections.Generic;

[Serializable]
public class SaveData {
	public List<string> levelsClearedOnNormal;
	public List<string> levelsClearedOnHard;
	public List<string> weaponsUnlocked;
	public int musicVolume;
	public int effectsVolume;
	public bool normalControlScheme;

	public SaveData() {
		levelsClearedOnNormal = new List<string>();
		levelsClearedOnHard = new List<string>();
		musicVolume = 50;
		effectsVolume = 50;
		normalControlScheme = true;

		levelsClearedOnNormal.Add("LEVEL 1");
		levelsClearedOnNormal.Add("LEVEL 2");
	}



}
