using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelPowerup : MonoBehaviour
{
    public PPManager ppmanager;
    public WaveSystem wavesystem;

    // Start is called before the first frame update
    void Start()
    {
        wavesystem = GameObject.FindGameObjectWithTag("WaveSystem").GetComponent<WaveSystem>();
        ppmanager = wavesystem.ppManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            wavesystem.StartCoroutine(wavesystem.WinPickup());
            Destroy(gameObject);
        }
    }
}
