using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour {

	public GameObject[][] weaponsList;
	private GameObject[] selectedWeapons;

	// Use this for initialization
	void Start () {
		int[] weaponsData = new int[6];
        weaponsData[0] = PlayerPrefs.GetInt("SelectedWeaponRed", 1);
        weaponsData[1] = PlayerPrefs.GetInt("SelectedWeaponOrange", 1);
        weaponsData[2] = PlayerPrefs.GetInt("SelectedWeaponYellow", 1);
        weaponsData[3] = PlayerPrefs.GetInt("SelectedWeaponGreen", 1);
        weaponsData[4] = PlayerPrefs.GetInt("SelectedWeaponBlue", 1);
        weaponsData[5] = PlayerPrefs.GetInt("SelectedWeaponPurple", 1);

		selectedWeapons = new GameObject[6];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
