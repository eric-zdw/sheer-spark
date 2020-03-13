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

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}

    // Update is called once per frame
    void Update() {
        if (player != null)
        {
            if (player.transform.position.x < transform.position.x + limitX && player.transform.position.x > transform.position.x - limitX
                && player.transform.position.y < transform.position.y + limitY && player.transform.position.y > transform.position.y - limitY)
            {
                safeSpawn = false;
            }
            else
            {
                safeSpawn = true;
            }
        }


        if (animationTimer <= 0f)
        {
            Instantiate(particles2, transform.position, Quaternion.identity);
            if (spawnTarget != null)
                Instantiate(spawnTarget, transform.position, Quaternion.identity);
            isSpawning = false;
            animationTimer = animTime;
        }

        if (isSpawning)
        {
            animationTimer -= Time.deltaTime;
        }

	}

    public void Spawn(GameObject obj)
    {
        spawnTarget = obj;
        isSpawning = true;
        Instantiate(particles, transform.position, Quaternion.identity);
    }
}
