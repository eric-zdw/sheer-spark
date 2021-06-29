using System;
using System.Collections.Generic;

[Serializable]
public class SaveData {
	public List<string> levelsClearedOnNormal;
	public List<string> levelsClearedOnHard;
	public List<int> scoresNormal;
	public List<int> scoresHard;
	public List<int> weaponsUnlocked;
	//public List<string> weaponLoadout;

	public SaveData() {
		levelsClearedOnNormal = new List<string>();
		levelsClearedOnHard = new List<string>();
		weaponsUnlocked = new List<int>();

		/*
		weaponLoadout = new List<string>();

		//default weapon loadout
		weaponLoadout.Add("WeaponRed1");
		weaponLoadout.Add("WeaponOrange1");
		weaponLoadout.Add("WeaponYellow1");
		weaponLoadout.Add("WeaponGreen1");
		weaponLoadout.Add("WeaponBlue1");
		weaponLoadout.Add("WeaponPurple1");
		*/
		
	}
}
