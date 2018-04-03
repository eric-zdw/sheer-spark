using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePowerup : MonoBehaviour {

    public GameObject redWeapon;
    public Material newColour;
    public GameObject explosion;
    public Material newMaterial;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 8f);
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
        GameObject newWeapon = Instantiate(redWeapon, player.transform.position, player.transform.rotation, parent);
        PlayerBehaviour playerScript = player.GetComponent<PlayerBehaviour>();
        playerScript.getPowerup(newWeapon, newColour, 4, newMaterial, "ORBITAL MANIPULATOR");
        Destroy(weapon);
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
