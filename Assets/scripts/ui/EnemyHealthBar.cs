using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {
    public GameObject target;
    private RectTransform rectTransform;
    private Enemy enemyScript;

    public float widthScale = 1f;
    public float heightScale = 1f;
    public float heightOffset = 0f;

    public void Initialize(GameObject t, float w, float h, float o)
    {
        target = t;
        widthScale = w;
        heightScale = h;
        heightOffset = o;
    }

    // Use this for initialization
    void Start () {
        enemyScript = target.GetComponent<Enemy>();
        rectTransform = GetComponent<RectTransform>();
}
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position + new Vector3(0, heightOffset, 0);
        rectTransform.sizeDelta = new Vector2(enemyScript.getHealth() * widthScale, heightScale);
	}
}
