using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> objects;
    public Vector2 horizontalRange;
    public float height;
    public float spawnRate;

    private float timer = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            int select = Random.Range(0, objects.Count);
            Instantiate(objects[select], new Vector3(Random.Range(horizontalRange.x, horizontalRange.y), height, 0f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            timer += spawnRate;
        }
    }
}
