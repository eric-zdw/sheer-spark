﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

	public List<GameObject> powerups;
    public GameObject[] weaponList;

	// Use this for initialization
	void Awake () {
        powerups[0] = weaponList[0];
        powerups[1] = weaponList[1];
        powerups[2] = weaponList[2];
        powerups[3] = weaponList[3];
        powerups[4] = weaponList[4];
        powerups[5] = weaponList[5];
	}

    void Update() {
        //print(powerups[3].name);
    }
}
