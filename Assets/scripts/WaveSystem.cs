using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSystem : MonoBehaviour {

    public WaveEntry[] waveEntries;
	public int waveNumber;
    public static float enemyPower = 1f;
    public float enemyPowerIncreasePerWave = 0.08f;
    public float easyPowerMultiplier = 0.5f;
    public float normalPowerMultiplier = 1f;
    public float hardPowerMultiplier = 2f;
    bool ppChange = false;
    public int currentStage;

    private Queue<EnemyEntry> enemiesInQueue;

    bool activeLevel = false;
    bool started = false;

    public GameObject[] enemies;

    private List<GameObject> spawners;
    private List<GameObject> airSpawners;
    private List<GameObject> groundSpawners;
    private List<Spawner> spawnerScripts;

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
    public SaveManager saveManager;
    public MusicManager musicManager;

    private bool gameFinished;

    public static bool isPaused = false;

    private float savedTimeScale = 1.5f;

    public GameObject pauseMenu;
    public GameObject gameUI;

    public bool isBossStage = false;
    public GameObject BossEnemy;

    // Use this for initialization
    void Start () {

        WaveSystem.isPaused = false;
        
        waveNumber = 0;
        enemyPower = 1f;

        InitializeSpawners();

        enemiesInQueue = new Queue<EnemyEntry>();
        InitializeEnemyQueue();

        currentStage = PlayerPrefs.GetInt("ActiveStage");
        
        actualProbabilities = new float[probabilities.Length];
        InitializeEnemyList();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    private void InitializeEnemyQueue() {
        if (waveEntries[waveNumber].SpawnInOrder) {
            foreach (EnemyData ed in waveEntries[waveNumber].Enemies) {
                //skip if infinite spawn
                if (ed.EnemyCount > 0) {
                    for (int i = 0; i < ed.EnemyCount; i++) {
                        enemiesInQueue.Enqueue(ed.Entry);
                    }
                }        
            }
        }
    }

    private IEnumerator SpawnRoutine() {
        while (enemiesInQueue.Count > 0) {
            Spawn();
            yield return new WaitForSeconds(0.1f);
        }
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

        foreach (Spawner s in spawnerScripts) {
            if (s.isGrounded) {
                groundSpawners.Add(s);
            }
            else {
                airSpawners.Add(s);
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
            delay = 5f;
            enemyPower = 1 + (waveNumber * enemyPowerIncreasePerWave);
            spawnInterval = Mathf.Sqrt(10 / Mathf.Sqrt(reserveEnemies));
            activeLevel = false;
            InitializeEnemyList();
        }

        
    }

    IEnumerator YouWin() {
        SaveManager.saveData.levelsClearedOnNormal.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        SaveManager.WriteToFile(SaveManager.saveData);
        wc.LevelCompleteRoutine();
        yield return new WaitForSecondsRealtime(10f);
        ReturnToMenu();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame() {
        WaveSystem.isPaused = true;
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        gameUI.SetActive(false);
    }

    public void UnpauseGame() {
        WaveSystem.isPaused = false;
        Time.timeScale = savedTimeScale;
        pauseMenu.SetActive(false);
        gameUI.SetActive(true);
    }
}
