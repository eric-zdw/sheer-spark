using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public GameObject target;
    public UnityEngine.UI.Image barImg;
    public RectTransform rect;
    public UnityEngine.UI.Image redBarImg;
    private Enemy enemyScript;
    public float offset = 0.5f;
    private float maxSize;


    private float velocity = 0f;
    private float drainSpeed = 0f;

	// Use this for initialization
	void Start () {
        barImg.fillAmount = 1f;
        redBarImg.fillAmount = 1f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        barImg.transform.position = target.transform.position + new Vector3(0f, (target.transform.localScale.y / 2f) * 2f, 0f);
        barImg.fillAmount = enemyScript.getHealth() / enemyScript.getMaxHealth();

        //Red portion of bar lerps to show damage taken
        redBarImg.transform.position = target.transform.position + new Vector3(0f, (target.transform.localScale.y / 2f) * 2f, 0f);
        redBarImg.fillAmount = Mathf.SmoothDamp(redBarImg.fillAmount, barImg.fillAmount, ref velocity, 0.5f);
	}

    public void setTarget(GameObject t)
    {
        target = t;
        enemyScript = target.GetComponent<Enemy>();


        drainSpeed = enemyScript.getMaxHealth() * 0.01f;
        maxSize = target.transform.localScale.x * 1.5f;
        rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(maxSize, maxSize * 0.1f, 0f);
    }
}
