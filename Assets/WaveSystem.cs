using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour {
    
    public int remainingEnemies;
    private int activeEnemies;
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
    private bool[] activeMusics;
    private bool musicChosen = false;
    private PlayerBehaviour player;
    private bool highIntensity = false;

    public float[] probabilities;
    private float[] actualProbabilities;
    private float totalProbability;
    public int[] allowedWaves;
    List<int> waveRoster;

    public bool gameStarted = false;
    private WaveComplete wc;
    private WaveComplete2 wc2;

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

        activeMusics = new bool[3];
        for (int i = 0; i < 3; i++)
        {
            activeMusics[i] = false;
        }

        waveRoster = new List<int>();
        actualProbabilities = new float[probabilities.Length];
        InitializeEnemyList();

        wc = GameObject.Find("WaveComplete").GetComponent<WaveComplete>();
        wc2 = GameObject.Find("WaveComplete2").GetComponent<WaveComplete2>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameStarted)
        {
            remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length + reserveEnemies;
            activeEnemies = remainingEnemies - reserveEnemies;

            for (int i = 0; i < 3; i++)
            {
                if (activeMusics[i] == false && musics[i].volume > 0f)
                {
                    musics[i].volume -= 0.15f * Time.deltaTime;
                }
                else if (activeMusics[i] == true && musics[i].volume < 0.5f)
                {
                    musics[i].volume += 0.15f * Time.deltaTime;
                }
            }

            delay -= Time.deltaTime;
            if (delay < 0f && started == false)
            {
                started = true;
                activeLevel = true;
            }
            else if (delay < 0f && started == true && activeLevel == false)
            {
                activeLevel = true;
            }

            if (delay < 0f && activeLevel == true && musicChosen == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    activeMusics[i] = false;
                }

                if (waveNumber < 3)
                    activeMusics[0] = true;
                else if (waveNumber < 5)
                    activeMusics[1] = true;
                else
                    activeMusics[1] = true;

                musicChosen = true;
                print("choosing music");
            }

            if (((player.HP == 1) || (activeEnemies > 12f)) && highIntensity == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    activeMusics[i] = false;
                }

                if (waveNumber < 3)
                    activeMusics[1] = true;
                else if (waveNumber < 5)
                    activeMusics[2] = true;
                else
                    activeMusics[2] = true;

                highIntensity = true;
                print("high intensity");
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
    }

    void ResetWave()
    {
        wc.StartRoutine();
        wc2.StartRoutine();
        waveNumber++;
        reserveEnemies = 5 + (int)(waveNumber * waveNumber * 0.75f);
        delay = 15f;
        enemyPower = 1 + (waveNumber * 0.05f);
        spawnInterval = Mathf.Sqrt(10 / Mathf.Sqrt(reserveEnemies));
        activeLevel = false;
        InitializeEnemyList();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>().HP = 3;
        musicChosen = false;
        highIntensity = false;

        for (int i = 0; i < 3; i++)
        {
            activeMusics[i] = false;
            print("disabling music");
        }
        activeMusics[0] = true;
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
}
