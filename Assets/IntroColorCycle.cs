using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroColorCycle : MonoBehaviour
{
    public Color[] colorList;
    public GameObject[] objects;
    public Light[] lights;
    public ParticleSystemRenderer[] particles;
    public float cycleSpeed = 5f;
    private int currentColor = 0;
    private float timer = 0f;

    private MaterialPropertyBlock mpb;

    // Start is called before the first frame update
    void Start()
    {
        mpb = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        while (timer > cycleSpeed)
        {
            currentColor++;
            /*
            if (currentColor >= colorList.Length)
            {
                currentColor = 0;
            }
            */

            timer -= cycleSpeed;
        }

        float lerp = timer / cycleSpeed;
        int current = currentColor % colorList.Length;
        int currentPlusOne = (currentColor + 1) % colorList.Length;

        float r = Mathf.SmoothStep(colorList[current].r, colorList[currentPlusOne].r, lerp);
        float g = Mathf.SmoothStep(colorList[current].g, colorList[currentPlusOne].g, lerp);
        float b = Mathf.SmoothStep(colorList[current].b, colorList[currentPlusOne].b, lerp);
        Color newColor = new Color(r, g, b, 1f);

        mpb.SetColor("_EmissionColor", newColor * 2f);

        foreach (GameObject obj in objects)
        {
            obj.GetComponent<MeshRenderer>().SetPropertyBlock(mpb);
        }

        foreach (Light l in lights)
        {
            l.color = newColor;
        }

        foreach (ParticleSystemRenderer p in particles)
        {
            p.SetPropertyBlock(mpb);
        }
    }
}
