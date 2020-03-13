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
            other.GetComponent<PlayerBehaviour>().takeDamage(10);
        }
        else if (other.tag == "Enemy") {
            other.GetComponent<Enemy>().getDamage(9999);
        }
    }
}
