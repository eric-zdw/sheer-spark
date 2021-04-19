using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLightsine : MonoBehaviour
{
    private Light l;
    public float strobeSpeed = 0.2f;
    public float intensity = 1.5f;
    public float floor = 0.5f;
    public float offset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        l = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        l.intensity = (((Mathf.Sin((Time.time * strobeSpeed) + (offset * Mathf.PI))) + 1f) * intensity) + floor;
    }
}
