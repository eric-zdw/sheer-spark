using System;
using System.Collections.Generic;

[Serializable]
public class SaveData {
	public List<string> levelsClearedOnNormal;
	public List<string> levelsClearedOnHard;
	public List<string> weaponsUnlocked;
	public List<string> weaponLoadout;

	public SaveData() {
		levelsClearedOnNormal = new List<string>();
		levelsClearedOnHard = new List<string>();
		weaponsUnlocked = new List<string>();
		weaponLoadout = new List<string>();

		//default weapon loadout
		weaponLoadout.Add("WeaponRed1");
		weaponLoadout.Add("WeaponOrange1");
		weaponLoadout.Add("WeaponYellow1");
		weaponLoadout.Add("WeaponGreen1");
		weaponLoadout.Add("WeaponBlue1");
		weaponLoadout.Add("WeaponPurple1");
		
	}
}
