using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    public PowerupType powerupType;
    public Material playerMaterial;
    public GameObject explosion;
    public Material newMaterial;
    
    private float timer = 20f;

	// Use this for initialization
	void Start () {
        //get current player weapon from settings.
        //weaponPrefab = 

        Destroy(gameObject, timer);
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 5f) {
            StartCoroutine(Fadeout());
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PowerUp(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void PowerUp(GameObject player)
    {
        GameObject oldWeapon = player.GetComponentInChildren<Weapon>().gameObject;
        Transform parent = oldWeapon.transform.parent;
        GameObject newWeapon = Instantiate(PowerupManager.GetPowerup(powerupType), player.transform.position, player.transform.rotation, parent);
        PlayerBehaviour playerScript = player.GetComponent<PlayerBehaviour>();
        playerScript.getPowerup(newWeapon, playerMaterial, (int)powerupType, newMaterial, "ORBITAL MANIPULATOR");
        Destroy(oldWeapon);
        Instantiate(explosion, transform.position, transform.rotation);
    }

    private IEnumerator Fadeout() {
        MaterialPropertyBlock mpb;
        while (true) {
            yield return new WaitForEndOfFrame();
        }
    }
}
