using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatBar : MonoBehaviour {

    RectTransform bar;
    PlayerBehaviour pb;

    // Use this for initialization
    void Start()
    {
        bar = GetComponent<RectTransform>();
        pb = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        bar.localScale = new Vector3(1f * (pb.powerupLevel / 7.5f), 1f, 1f);
    }
}
