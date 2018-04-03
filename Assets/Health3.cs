using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health3 : MonoBehaviour {
     
    PlayerBehaviour player;
    public UnityEngine.UI.Image damageAlert;
    private UnityEngine.UI.Image hp;
    bool isActive = true;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
        hp = GetComponent<UnityEngine.UI.Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (player.HP < 3 && isActive == true)
        {
            damageAlert.color = new Color(255, 128, 128, 0.25f);
            hp.enabled = false;
            isActive = false;
        }
        else if (player.HP >= 3 && isActive == false)
        {
            hp.enabled = true;
            isActive = true;
        }
            
	}
}
