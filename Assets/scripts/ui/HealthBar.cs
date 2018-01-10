using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public GameObject target;
    public RectTransform rect;
    private Enemy enemyScript;
    public float offset = 0.5f;
    private float maxSize;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update ()
    {
        rect.transform.position = target.transform.position + new Vector3(0f, (target.transform.localScale.y / 2f) * 2f, 0f);
        rect.localScale = new Vector3(maxSize * enemyScript.getHealth() / enemyScript.getMaxHealth(), maxSize * 0.1f, 0f);
	}

    public void setTarget(GameObject t)
    {
        target = t;
        enemyScript = target.GetComponent<Enemy>();

        maxSize = target.transform.localScale.x * 1.5f;
        rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(maxSize, maxSize * 0.1f, 0f);
    }
}
