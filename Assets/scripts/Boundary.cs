using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

    private GameObject player;
    public Material boundaryMat;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float sensitivity;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        boundaryMat.SetColor("_TintColor", new Color(0, 0, 0, 1));
        if (player.transform.position.x < minX)
        {
            float newColor = Mathf.Abs(player.transform.position.x - minX) * sensitivity;
            boundaryMat.SetColor("_TintColor", new Color(newColor, 0, 0, 1));
        }
        if (player.transform.position.x > maxX)
        {
            float newColor = Mathf.Abs(player.transform.position.x - maxX) * sensitivity;
            boundaryMat.SetColor("_TintColor", new Color(newColor, 0, 0, 1));
        }
        if (player.transform.position.y < minY)
        {
            float newColor = Mathf.Abs(player.transform.position.y - minY) * sensitivity;
            boundaryMat.SetColor("_TintColor", new Color(newColor, 0, 0, 1));
        }
        if (player.transform.position.y > maxY)
        {
            float newColor = Mathf.Abs(player.transform.position.y - maxY) * sensitivity;
            boundaryMat.SetColor("_TintColor", new Color(newColor, 0, 0, 1));
        }
    }
}
