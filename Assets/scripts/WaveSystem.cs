using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSystem : MonoBehaviour {
    
    public int remainingEnemies;
    private int activeEnemies;
    public int reserveEnemies;
    public int beginningEnemies = 3;
	public int waveNumber;
    public float jumpLimitY;
    public float enemyPower;
    bool ppChange = false;
    public int currentStage;


    float delay = 10f;

    private float spawnInterval;
    float spawnDelay;

    bool activeLevel = false;
    bool started = false;

    public GameObject[] enemies;

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

    public bool gameStarted = false;
    public WaveComplete wc;
    public WaveComplete2 wc2;

    public int maxWaves = 10;

    public PPManager ppManager;

    private bool gameFinished;

    // Use this for initialization
    void Start () {

        InitializeWaveParameters();
        InitializeSpawners();

        currentStage = PlayerPrefs.GetInt("ActiveStage");

        musics = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
        lastMusic = musics[0];
        newMusic = musics[0];
        activeMusics = new bool[3];
        for (int i = 0; i < 3; i++)
        {
            activeMusics[i] = false;
        }
        
        actualProbabilities = new float[probabilities.Length];
        InitializeEnemyList();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void InitializeWaveParameters()
    {
        waveNumber = 1;
        reserveEnemies = 6;
        enemyPower = 1f;
        spawnInterval = 3f;
        spawnDelay = spawnInterval;
    }

    void InitializeSpawners()
    {
        spawners = new List<GameObject>();
        spawners.AddRange(GameObject.FindGameObjectsWithTag("Spawner"));
        spawnerScripts = new List<Spawner>();
        for (int i = 0; i < spawners.Count; i++)
        {
            spawnerScripts.Add(spawners[i].GetComponent<Spawner>());
        }
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

			//activate high-intensity music if player is damaged or many enemies remain
            if (((player.HP == 2) || (activeEnemies > 8f)) && highIntensity == false)
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

            if (remainingEnemies == 0 && gameFinished == false)
                IncrementWave();
        }

    }

    IEnumerator UpdateEnemyCount()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
        }
    }

    void IncrementWave()
    {
        for (int i = 0; i < 3; i++)
        {
            activeMusics[i] = false;
            print("disabling music");
        }
        activeMusics[0] = true;

        //Stop high-intensity music.
        musicChosen = false;
        highIntensity = false;

        waveNumber++;
        if (waveNumber > maxWaves)
        {
            gameFinished = true;
            StartCoroutine(ppManager.GameEndEffects());
            StartCoroutine(YouWin());
        }
        else {
            //Start slow down and post-processing effects.
            StartCoroutine(ppManager.SlowDown());
            StartCoroutine(ppManager.ChangePP());
            //TODO: add music sting, or cut music entirely after completion.            

            StartCoroutine(wc.WaveCompleteRoutine());
            wc2.StartRoutine();
        
            reserveEnemies = beginningEnemies + (int)(Mathf.Pow(waveNumber, 1.5f));
            delay = 15f;
            enemyPower = 1 + (waveNumber * 0.05f);
            spawnInterval = Mathf.Sqrt(10 / Mathf.Sqrt(reserveEnemies));
            activeLevel = false;
            InitializeEnemyList();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>().HP = 3;
        }

        
    }

    IEnumerator YouWin() {
        PlayerPrefs.SetInt("Stage" + currentStage + "Complete", 1);
        wc.LevelCompleteRoutine();
        yield return new WaitForSecondsRealtime(10f);
        StartCoroutine(ReturnToMenu());
    }

    IEnumerator ReturnToMenu()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(0);
        while(!load.isDone)
        {
            yield return null;
        }
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
