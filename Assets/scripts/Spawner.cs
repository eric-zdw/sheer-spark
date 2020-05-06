using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private GameObject spawnTarget;
    public GameObject particles;
    public GameObject particles2;
    public float limitX;
    public float limitY;

    private float timer;
    private float animTime = 0.5f;
    private float animationTimer = 0.5f;
    private bool isSpawning;
    private GameObject player;

    public bool safeSpawn;
    public bool isGrounded;
    public bool isBusy;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        if (Physics.Raycast(transform.position, Vector3.down, 4f)) {
            isGrounded = true;
        }
	}

    // Update is called once per frame
    void FixedUpdate() {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 20f) {
                safeSpawn = false;
            }
            else {
                safeSpawn = true;
            }
        }
	}

    private IEnumerator Spawn(GameObject obj) {
        isBusy = true;
        spawnTarget = obj;
        isSpawning = true;
        float timer = 2f;
        Instantiate(particles, transform.position, Quaternion.identity);
        while (timer > 0f) {    
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Instantiate(particles2, transform.position, Quaternion.identity);
        Instantiate(spawnTarget, transform.position, Quaternion.identity);
        isBusy = false;
    }
}
