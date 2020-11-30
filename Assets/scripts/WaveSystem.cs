using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class IListExtensions {
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}

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

    private List<EnemyEntry> enemyList;
    private List<EnemyEntry> infiniteEnemyList;
    public int enemiesInLimbo = 0;

    bool activeLevel = false;
    bool started = false;

    private List<GameObject> spawners;
    private List<Spawner> groundSpawners;
    private List<Spawner> airSpawners;
    private List<Spawner> spawnerScripts;

    private PlayerBehaviour player;
    private bool highIntensity = false;

    public bool gameStarted = false;
    public WaveComplete wc;
    public WaveComplete2 wc2;

    public PPManager ppManager;
    public SaveManager saveManager;
    public MusicManager musicManager;

    private bool gameFinished;

    public static bool isPaused = false;

    private float savedTimeScale = 1.5f;

    public GameObject pauseMenu;
    public GameObject gameUI;

    private int activeEnemies = 0;

    // Use this for initialization
    void Start () {
        WaveSystem.isPaused = false;
        
        waveNumber = 0;
        enemyPower = 1f;

        enemyList = new List<EnemyEntry>();
        infiniteEnemyList = new List<EnemyEntry>();
        InitializeEnemyQueue();

        InitializeSpawners();

        currentStage = PlayerPrefs.GetInt("ActiveStage");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        StartCoroutine(SpawnRoutine());
    }

    private void InitializeEnemyQueue() {
        foreach (EnemyData ed in waveEntries[waveNumber].Enemies) {
            if (ed.EnemyCount >= 0) {
                for (int i = 0; i < ed.EnemyCount; i++) {
                    enemyList.Add(ed.Entry);
                }
            }
            //if EnemyCount < 0, add enemy to infinite spawn queue
            else {
                infiniteEnemyList.Add(ed.Entry);
            }        
        }
        //randomize enemies if needed
        //Fisher-Yates shuffle
        if (!waveEntries[waveNumber].SpawnInOrder) {
            enemyList.Shuffle();
        }
    }

    private IEnumerator SpawnRoutine() {
        //wait for game start
        while (!gameStarted) {
            yield return new WaitForEndOfFrame();
        }

        //play start music
        musicManager.StartMusic();
        StartCoroutine(musicManager.ChangeMusic(0));
        StartCoroutine(ppManager.SlowDown());
        StartCoroutine(ppManager.ChangePP());

        while (true) {
            //spawn initial enemies
            for (int i = 0; i < waveEntries[waveNumber].InitialSpawns; i++) {
                Spawn();
            }
            yield return new WaitForSeconds(waveEntries[waveNumber].SpawnRate);

            //main spawn routine
            while (enemyList.Count > 0) {
                //if at maximum allowed active enemies, wait until enemy is destroyed
                while (activeEnemies > waveEntries[waveNumber].MaxActiveEnemies) {
                    yield return new WaitForEndOfFrame();
                }

                Spawn();

                yield return new WaitForSeconds(waveEntries[waveNumber].SpawnRate);
            }

            //stay until enemyList is reinitialized
            while (enemyList.Count == 0) {
                yield return new WaitForEndOfFrame();
            }

            // brief wait before next wave
            yield return new WaitForSeconds(1.5f);
        }
    }

    void Spawn()
    {
        List<Spawner> availableSpawners = new List<Spawner>();
        for (int i = 0; i < spawners.Count; i++)
        {
            if (spawnerScripts[i].SafeToSpawn())
            {
                availableSpawners.Add(spawnerScripts[i]);
            }
        }

        int rngSpawner = Random.Range(0, availableSpawners.Count);
        StartCoroutine(availableSpawners[rngSpawner].Spawn(enemyList[0].Object));
        StartCoroutine(EnemyInLimbo());
        enemyList.RemoveAt(0);
    }

    private IEnumerator EnemyInLimbo() {
        enemiesInLimbo++;
        yield return new WaitForSeconds(2.5f);
        enemiesInLimbo--;
    }

    void InitializeSpawners()
    {
        spawners = new List<GameObject>();
        spawners.AddRange(GameObject.FindGameObjectsWithTag("Spawner"));
        spawnerScripts = new List<Spawner>();
        groundSpawners = new List<Spawner>();
        airSpawners = new List<Spawner>();
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
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isPaused) PauseGame();
            else UnpauseGame();
        }

        activeEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        int remainingEnemies = activeEnemies + enemiesInLimbo + enemyList.Count;

        if (gameStarted)
        {
            if (remainingEnemies == 0 && gameFinished == false)
                IncrementWave();
        }

    }

    void IncrementWave()
    {
        waveNumber++;
        if (waveNumber == waveEntries.Length)
        {
            StartCoroutine(musicManager.ChangeMusic(5));
            //Game is done. Increment stats and end game.
            gameFinished = true;
            StartCoroutine(ppManager.GameEndEffects());
            StartCoroutine(YouWin());
        }

        else {
            if (waveNumber == 1) {
                StartCoroutine(musicManager.ChangeMusic(1));
            }
            else if (waveNumber == 3) {
                StartCoroutine(musicManager.ChangeMusic(2));
            }
            else if (waveNumber == 4) {
                StartCoroutine(musicManager.ChangeMusic(3));
            }
            else if (waveNumber == 6) {
                StartCoroutine(musicManager.ChangeMusic(4));
            }
            //Start slow down and post-processing effects.
            StartCoroutine(ppManager.SlowDown());
            StartCoroutine(ppManager.ChangePP());
            //TODO: add music sting, or cut music entirely after completion.            

            InitializeEnemyQueue();
            //StartCoroutine(wc.WaveCompleteRoutine());
            wc2.StartRoutine();
        }

        
    }

    IEnumerator YouWin() {
        SaveManager.saveData.levelsClearedOnNormal.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        SaveManager.WriteToFile(SaveManager.saveData);
        //wc.LevelCompleteRoutine();
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
