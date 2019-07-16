using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathBoundary : MonoBehaviour {

    private GameObject player;
    public Material boundaryMat;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float sensitivity;

    private float flashValue = 0f;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        boundaryMat.SetColor("_TintColor", new Color(0, 0, 0, 1));
        float colorValue = 0f;
        if (player.transform.position.x < minX)
        {
            float newColor = Mathf.Abs(player.transform.position.x - minX) * sensitivity;
            colorValue = Mathf.Max(newColor, colorValue);
        }
        if (player.transform.position.x > maxX)
        {
            float newColor = Mathf.Abs(player.transform.position.x - maxX) * sensitivity;
            colorValue = Mathf.Max(newColor, colorValue);
        }
        if (player.transform.position.y < minY)
        {
            float newColor = Mathf.Abs(player.transform.position.y - minY) * sensitivity;
            colorValue = Mathf.Max(newColor, colorValue);
        }
        if (player.transform.position.y > maxY)
        {
            float newColor = Mathf.Abs(player.transform.position.y - maxY) * sensitivity;
            colorValue = Mathf.Max(newColor, colorValue);
        }
        
        colorValue += flashValue;
        boundaryMat.SetColor("_TintColor", new Color(colorValue, colorValue * 0.25f, colorValue * 0.25f, 0.1f + flashValue));
        
        if (flashValue > 0) flashValue -= Time.deltaTime; else flashValue = 0;
    }

    public void HitFlash() {
        flashValue = 0.5f;
    }
}
