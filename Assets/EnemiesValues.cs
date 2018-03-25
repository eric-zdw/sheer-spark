using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesValues : MonoBehaviour
{

    UnityEngine.UI.Text text;
    WaveSystem ws;

    bool isEnabled = false;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<UnityEngine.UI.Text>();
        text.enabled = false;

        ws = GameObject.FindGameObjectWithTag("WaveSystem").GetComponent<WaveSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ws.reserveEnemies == 0)
        {
            text.enabled = true;
            isEnabled = true;

            text.text = ws.remainingEnemies.ToString();
        }
        else if (ws.reserveEnemies != 0 && isEnabled == true)
        {
            text.enabled = false;
            isEnabled = false;
        }
    }
}
