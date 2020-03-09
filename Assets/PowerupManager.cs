using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

	public GameObject[] powerups;
    public GameObject[] weaponList;

	// Use this for initialization
	void Start () {
		powerups = new GameObject[6]; 
        powerups[0] = weaponList[PlayerPrefs.GetInt("RedWeapon")];
        powerups[1] = weaponList[PlayerPrefs.GetInt("OrangeWeapon")];
        powerups[2] = weaponList[PlayerPrefs.GetInt("YellowWeapon")];
        powerups[3] = weaponList[PlayerPrefs.GetInt("GreenWeapon")];
        powerups[4] = weaponList[PlayerPrefs.GetInt("BlueWeapon")];
        powerups[5] = weaponList[PlayerPrefs.GetInt("PurpleWeapon")];
	}
}
