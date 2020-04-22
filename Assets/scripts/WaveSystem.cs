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
    public static float enemyPower = 1f;
    public float enemyPowerIncreasePerWave = 0.08f;
    public float easyPowerMultiplier = 0.5f;
    public float normalPowerMultiplier = 1f;
    public float hardPowerMultiplier = 2f;
    bool ppChange = false;
    public int currentStage;

    public int[] cameraBoundaries;

    private int spawningEnemies = 0;


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

    public static bool isPaused = false;

    private float savedTimeScale = 1.5f;

    public GameObject pauseMenu;

    public bool isBossStage = false;
    public GameObject BossEnemy;

    public SaveManager saveManager;

    // Use this for initialization
    void Start () {

        WaveSystem.isPaused = false;
        InitializeWaveParameters();
        InitializeSpawners();

        currentStage = PlayerPrefs.GetInt("ActiveStage");

        musics = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
        lastMusic = musics[0];
        newMusic = musics[0];
        activeMusics = new bool[5];
        for (int i = 0; i < 5; i++)
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
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isPaused) PauseGame();
            else UnpauseGame();
        }

        if (gameStarted)
        {
            remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length + reserveEnemies;
            activeEnemies = remainingEnemies + spawningEnemies - reserveEnemies;

            for (int i = 0; i < 5; i++)
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
                for (int i = 0; i < 5; i++)
                {
                    activeMusics[i] = false;
                }

                if (waveNumber < 2)
                    activeMusics[0] = true;
                else if (waveNumber < 4)
                    activeMusics[1] = true;
                else if (waveNumber < 6)
                    activeMusics[2] = true;
                else if (waveNumber < 8)
                    activeMusics[3] = true;
                else
                    activeMusics[4] = true;

                musicChosen = true;
                print("choosing music");
            }

			//activate high-intensity music if player is damaged or many enemies remain
            /*
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
            */

            if (activeLevel == true && reserveEnemies != 0)
            {
                spawnDelay -= Time.deltaTime;
                if (spawnDelay < 0f)
                {
                    Spawn();
                    reserveEnemies--;
                    spawningEnemies++;
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
        /*
        for (int i = 0; i < 5; i++)
        {
            activeMusics[i] = false;
            print("disabling music");
        }
        */
        //activeMusics[0] = true;

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
            enemyPower = 1 + (waveNumber * enemyPowerIncreasePerWave);
            spawnInterval = Mathf.Sqrt(10 / Mathf.Sqrt(reserveEnemies));
            activeLevel = false;
            InitializeEnemyList();
        }

        
    }

    IEnumerator YouWin() {
        saveManager.saveData.levelsClearedOnNormal.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        SaveManager.WriteToFile(saveManager.saveData);
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
                spawningEnemies--;
            }
            counter++;
        }
    }

    public void PauseGame() {
        WaveSystem.isPaused = true;
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void UnpauseGame() {
        WaveSystem.isPaused = false;
        Time.timeScale = savedTimeScale;
        pauseMenu.SetActive(false);
    }
}
