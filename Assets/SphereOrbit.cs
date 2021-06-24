using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereOrbit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public GameObject sphere1;
    public GameObject sphere2;
    public float distance;

    private float angle = 0f;
    private float velocity = 0f;
    private float accel = 0f;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (WaveSystem.player.transform.position.x >= transform.position.x)
        {
            accel = 3f;
        }
        else
        {
            accel = -3f;
        }

        velocity *= 0.98f;
        velocity += accel * Time.deltaTime;

        angle += velocity * Time.deltaTime;


        Vector3 addition = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        sphere1.transform.position = target.transform.position + (addition * distance);
        sphere2.transform.position = target.transform.position - (addition * distance);
    }
}
