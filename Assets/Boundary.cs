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
	void Update () {
        boundaryMat.SetColor("_TintColor", new Color(0, 0, 0, 1));
        if (player.transform.position.x < minX)
        {
            float newRed = Mathf.Abs(player.transform.position.x - minX) * sensitivity;
            print("newRed: " + newRed);
            boundaryMat.SetColor("_TintColor", new Color(newRed, 0, 0, 1));
        }
        if (player.transform.position.x > maxX)
        {
            float newRed = Mathf.Abs(player.transform.position.x - maxX);
            boundaryMat.SetColor("_TintColor", new Color(newRed, 0, 0, 1));
        }
        if (player.transform.position.y < minY)
        {
            float newRed = Mathf.Abs(player.transform.position.y - minY);
            boundaryMat.SetColor("_TintColor", new Color(newRed, 0, 0, 1));
        }
        if (player.transform.position.y > maxY)
        {
            float newRed = Mathf.Abs(player.transform.position.y - maxY);
            boundaryMat.SetColor("_TintColor", new Color(newRed, 0, 0, 1));
        }
    }
}
