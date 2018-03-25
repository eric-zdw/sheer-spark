using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health3 : MonoBehaviour {
     
    PlayerBehaviour player;
    public UnityEngine.UI.Image damageAlert;
    bool isActive = true;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player.HP < 3 && isActive == true)
        {
            damageAlert.color = new Color(255, 128, 128, 0.25f);
            gameObject.SetActive(false);
            isActive = false;
        }
        else if (isActive == false)
        {
            gameObject.SetActive(true);
            isActive = true;
        }
            
	}
}
