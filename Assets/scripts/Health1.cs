using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health1 : MonoBehaviour
{

    PlayerBehaviour player;
    public UnityEngine.UI.Image damageAlert;
    private UnityEngine.UI.Image hp;

    private float alertTimer = 0.2f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
        hp = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            damageAlert.color = new Color(255, 128, 128, 0.25f);
            hp.enabled = false;
        }
        else if (player.HP < 2)
        {
            alertTimer -= Time.deltaTime;
            if (alertTimer <= 0f)
            {
                if (hp.color == new Color(1f, 0.25f, 0.25f, 0.75f))
                    hp.color = new Color(1f, 1f, 1f, 0.75f);
                else
                    hp.color = new Color(1f, 0.25f, 0.25f, 0.75f);
                alertTimer = 0.2f;
            }

            hp.enabled = true;
        }
        else
        {
            hp.enabled = true;
        }

    }
}
