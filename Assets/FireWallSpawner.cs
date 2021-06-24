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
        bool hitWall = false;
        float wait = delay;
        while (numWalls < maxWalls && !hitWall)
        {
            if (Physics.CheckSphere(spawnPoint, 0.2f, 1 << 17))
            {
                Vector3 pos = spawnPoint;
                int attempts = 0;
                while (Physics.CheckSphere(pos, 0.2f, 1 << 17) && attempts < 10)
                {
                    pos += Vector3.up * 0.2f;
                    attempts++;
                }
                if (attempts == 10)
                {
                    hitWall = true;
                }
                spawnPoint = pos;
            }
            
            Instantiate(fireWallPrefab, spawnPoint, Quaternion.identity);

            while (wait > 0f)
            {
                yield return new WaitForFixedUpdate();
                wait -= Time.fixedDeltaTime;
            }
            wait += delay;

            if (direction == 0)
            {
                spawnPoint -= Vector3.right * distanceBetweenWalls;
            }
            else
            {
                spawnPoint += Vector3.right * distanceBetweenWalls;
            }
            
            numWalls++;
        }
        
    }
}
