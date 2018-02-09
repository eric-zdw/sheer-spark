using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject spawnTarget;
    public GameObject particles;
    public GameObject particles2;
    public float limitX;
    public float spawnRate;

    private float timer;
    private float animTime = 0.5f;
    private float animationTimer;
    private bool isSpawning;
    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        timer = spawnRate;
	}

    // Update is called once per frame
    void Update() {

        
        if (timer <= 0f) {
            if (player.transform.position.x < transform.position.x + limitX && player.transform.position.x > transform.position.x - limitX)
            {
                timer = spawnRate;
            }
            else
            {
                if (isSpawning == false)
                    Spawn();
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (animationTimer <= 0f)
        {
            Instantiate(particles2, transform.position, Quaternion.identity);
            Instantiate(spawnTarget, transform.position, Quaternion.identity);
            isSpawning = false;
            timer = spawnRate;
            animationTimer = animTime;
        }

        if (isSpawning)
        {
            animationTimer -= Time.deltaTime;
        }


        


	}

    private void Spawn()
    {
        isSpawning = true;
        Instantiate(particles, transform.position, Quaternion.identity);
    }
}
