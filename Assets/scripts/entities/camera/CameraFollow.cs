using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {
    public GameObject followTarget;
    public int CameraDistance;
    Camera cam;

	private float shakeFactor = 0f;
    public float shakeDecayFactor;
    private Vector3 lastPosition;

    private float resetTimer = 5f;
    private bool hasRestarted = false;

	// Use this for initialization
	void Start ()
    {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		/*
        Vector3 newPosition;
        //inital camera position
        if (followTarget != null)
        {
            newPosition = followTarget.transform.position + new Vector3(0, 0, -CameraDistance);
            lastPosition = newPosition;
        }
        else
            newPosition = lastPosition;

        //add mouse positioning
        newPosition += cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraDistance));
        newPosition = new Vector3(newPosition.x / 2, newPosition.y / 2, -CameraDistance);
        transform.position = newPosition;
        
        if (shakeFactor >= 0.0001f)
        {
            transform.position += (Vector3)(Random.insideUnitCircle * shakeFactor);
        }
        shakeFactor *= shakeDecayFactor;
        */

        if (followTarget == null)
        {
            print("about to reset...");
            resetTimer -= Time.deltaTime;
            if (resetTimer < 0f && !hasRestarted)
            {
                StartCoroutine(ResetScene());
                hasRestarted = true;
            }
        }
    }

    IEnumerator ResetScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(0);
        while(!load.isDone)
        {
            yield return null;
        }
    }
}
