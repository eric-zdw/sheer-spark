using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathBoundary : MonoBehaviour {

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            for (int i = 0; i < 3; i++) {
                other.GetComponent<PlayerBehaviour>().takeDamage(10);
            }
        }
        else if (other.tag == "Enemy") {
            if (other.name != "OrangeBossHead" && other.name != "OrangeBossPiece") {
                other.GetComponent<Enemy>().getDamage(1000);
            }
        }
    }
}
