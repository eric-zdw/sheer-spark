using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : MonoBehaviour
{
    // Start is called before the first frame update
    Enemy target;
    private float timer;
    public float burnInterval = 1f;
    private float adjustedBurnInterval;
    public float burnDamage = 1f;
    public int burnStacks = 6;

    public int burnDuration = 5;
    private int burnCounter = 0;

    void Start()
    {
        target = transform.parent.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        adjustedBurnInterval = burnInterval / (1f + 0.25f * burnStacks);
        while (timer > adjustedBurnInterval)
        {
            timer -= adjustedBurnInterval;
            if (burnStacks > 0)
            {
                target.getDamage(burnDamage * (1f + burnStacks * 0.005f));

                burnStacks -= 1;
                /*
                burnCounter += 1;
                if (burnCounter >= burnDuration)
                {
                    burnCounter = 0;
                    burnStacks -= 1;
                }
                */
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
