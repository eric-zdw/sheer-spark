using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAlert : MonoBehaviour {

    UnityEngine.UI.Image damageAlert;

	// Use this for initialization
	void Start () {
        damageAlert = GetComponent<UnityEngine.UI.Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (damageAlert.color.a > 0f)
            damageAlert.color = new Color(255, 0, 0, damageAlert.color.a - 0.5f * Time.deltaTime);
	}
}
