using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangePowerup : MonoBehaviour {

    public GameObject redWeapon;
    public Material newColour;
    public GameObject explosion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("OK");
            PowerUp(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void PowerUp(GameObject player)
    {
        GameObject weapon = player.GetComponentInChildren<Weapon>().gameObject;
        Transform parent = weapon.transform.parent;
        print(weapon.tag);
        GameObject newWeapon = Instantiate(redWeapon, player.transform.position, player.transform.rotation, parent);
        PlayerBehaviour playerScript = player.GetComponent<PlayerBehaviour>();
        playerScript.RelinkWeapon(newWeapon);
        Destroy(weapon);

        playerScript.ChangeColour(newColour);
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
