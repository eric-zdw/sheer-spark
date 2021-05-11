using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallSpawner : MonoBehaviour
{
    public int direction;
    public int maxWalls;
    public float maxVerticalStep;
    public float distanceBetweenWalls;
    public float delay;

    public GameObject fireWallPrefab;
    private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        int numWalls = 0;
        while (numWalls < maxWalls)
        {
            Instantiate(fireWallPrefab, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(delay);
            spawnPoint += Vector3.right * distanceBetweenWalls;
            numWalls++;
        }
        
    }
}
