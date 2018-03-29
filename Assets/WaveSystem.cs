using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour {
    
    public int remainingEnemies;
    public int reserveEnemies;
    public int waveNumber;
    public float enemyPower;

    
    float delay = 15f;

    private float spawnInterval;
    float spawnDelay;

    bool activeLevel = false;
    bool started = false;

    public GameObject[] enemies;
    

    public GameObject[] uiElements;
    private List<GameObject> spawners;
    private List<Spawner> spawnerScripts;

    AudioSource lastMusic;
    AudioSource newMusic;

    public AudioSource[] musics;

    public float[] probabilities;
    private float[] actualProbabilities;
    private float totalProbability;
    public int[] allowedWaves;
    List<int> waveRoster;

    // Use this for initialization
    void Start () {
        waveNumber = 1;
        reserveEnemies = 6;
        enemyPower = 1f;
        spawnInterval = 3f;
        spawnDelay = spawnInterval;

        musics = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
        lastMusic = musics[0];
        newMusic = musics[0];

        spawners = new List<GameObject>();
        spawners.AddRange(GameObject.FindGameObjectsWithTag("Spawner"));
        spawnerScripts = new List<Spawner>();
        for (int i = 0; i < spawners.Count; i++)
        {
            spawnerScripts.Add(spawners[i].GetComponent<Spawner>());
        }

        SetMusic(musics[0]);
        waveRoster = new List<int>();
        actualProbabilities = new float[probabilities.Length];
        InitializeEnemyList();
    }
	
	// Update is called once per frame
	void Update () {
        remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length + reserveEnemies;
        //print("remaining enemies: " + remainingEnemies + ", reserve: " + reserveEnemies);

        delay -= Time.deltaTime;
        if (delay < 0f && started == false)
        {
            for (int i = 0; i < uiElements.Length; i++)
            {
                uiElements[i].SetActive(true);
            }
            started = true;
            activeLevel = true;
            SetMusic(musics[1]);
        }
        else if (delay < 0f && started == true && activeLevel == false)
        {
            activeLevel = true;
        }

        if (activeLevel == true && reserveEnemies != 0)
        {
            spawnDelay -= Time.deltaTime;
            if (spawnDelay < 0f)
            {
                Spawn();
                reserveEnemies--;
                spawnDelay = spawnInterval;
            }
        }

        if (remainingEnemies == 0)
            ResetWave();
    }

    void ResetWave()
    {
        waveNumber++;
        reserveEnemies = 5 + (waveNumber * waveNumber);
        delay = 15f;
        enemyPower = 1 + (waveNumber * 0.05f);
        spawnInterval = Mathf.Sqrt(30 / reserveEnemies);
        activeLevel = false;
        InitializeEnemyList();
    }

    void InitializeEnemyList()
    {
        totalProbability = 0f;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (allowedWaves[i] <= waveNumber)
            {
                actualProbabilities[i] = probabilities[i] + totalProbability;
                totalProbability += probabilities[i];
            }
            print("probability of enemy " + i + "is " + actualProbabilities[i]);
        }
    }

    void Spawn()
    {
        List<Spawner> availableSpawners = new List<Spawner>();
        for (int i = 0; i < spawners.Count; i++)
        {
            if (spawnerScripts[i].safeSpawn == true)
            {
                availableSpawners.Add(spawnerScripts[i]);
            }
        }

        int rngSpawner = Random.Range(0, availableSpawners.Count);
        float rngEnemy = Random.Range(1f, totalProbability);
        int counter = 0;
        bool hasSpawned = false;
        while(!hasSpawned)
        {
            if (rngEnemy < actualProbabilities[counter])
            {
                availableSpawners[rngSpawner].Spawn(enemies[counter]);
                hasSpawned = true;
            }
            counter++;
        }
    }

    void SetMusic(AudioSource track)
    {
        lastMusic.volume = 0f;
        lastMusic = track;
        track.volume = 0.25f;
    }
}
