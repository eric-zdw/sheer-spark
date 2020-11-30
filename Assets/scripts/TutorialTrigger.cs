using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    public GameObject[] powerups;
    private GameObject powerup;

    public bool isHelping = false;

    private float replaceInterval = 0.5f;
    private float replaceTimer;

	// Use this for initialization
	void Start () {
        powerup = Instantiate(powerups[Random.Range(0, 6)], transform.position, Quaternion.identity);
        Destroy(powerup, replaceInterval);
        replaceTimer = replaceInterval;
	}
	
	// Update is called once per frame
	void Update () {
        replaceTimer -= Time.deltaTime;
        if (replaceTimer <= 0)
        {
            powerup = Instantiate(powerups[Random.Range(0, 6)], transform.position, Quaternion.identity);
            Destroy(powerup, replaceInterval);
            replaceTimer = replaceInterval;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!isHelping)
                isHelping = true;
            else
                isHelping = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("WaveSystem").GetComponent<WaveSystem>().gameStarted = true;
            //GameObject.Find("Get Ready").GetComponent<Ready>().gameStarted = true;
            Destroy(gameObject);
        }
    }
}
