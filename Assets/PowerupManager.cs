using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PowerupType {
    Red, Orange, Yellow, Green, Blue, Purple
}

public class PowerupManager : MonoBehaviour {

    public PowerupsScriptableObject powerupList;
    static public GameObject[] activeWeapons;

    void Start() {
        int[] weaponPrefs = new int[6];
        weaponPrefs[0] = PlayerPrefs.GetInt("RedWeapon", 0);
        weaponPrefs[1] = PlayerPrefs.GetInt("OrangeWeapon", 0);
        weaponPrefs[2] = PlayerPrefs.GetInt("YellowWeapon", 0);
        weaponPrefs[3] = PlayerPrefs.GetInt("GreenWeapon", 0);
        weaponPrefs[4] = PlayerPrefs.GetInt("BlueWeapon", 0);
        weaponPrefs[5] = PlayerPrefs.GetInt("PurpleWeapon", 0);

        activeWeapons = new GameObject[6];
        activeWeapons[0] = powerupList.redWeapons[weaponPrefs[0]];
        activeWeapons[1] = powerupList.orangeWeapons[weaponPrefs[1]];
        activeWeapons[2] = powerupList.yellowWeapons[weaponPrefs[2]];
        activeWeapons[3] = powerupList.greenWeapons[weaponPrefs[3]];
        activeWeapons[4] = powerupList.blueWeapons[weaponPrefs[4]];
        activeWeapons[5] = powerupList.purpleWeapons[weaponPrefs[5]];
    }

    public static GameObject GetPowerup(PowerupType powerupType) {
        return activeWeapons[(int)powerupType];
    }
}
