using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {
    public GameObject followTarget;
    public int CameraDistance;
    Camera cam;

	public float shakeFactor = 0f;
    public float shakeMultiplier = 1f;
    public float shakeDecayFactor;
    private Vector3 lastPosition;

    private float resetTimer = 5f;
    private bool hasRestarted = false;

    public WaveSystem ws;

	// Use this for initialization
	void Start ()
    {
        followTarget = GameObject.Find("Player");
        cam = GetComponent<Camera>();
        ws = GameObject.FindGameObjectWithTag("WaveSystem").GetComponent<WaveSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 newPosition;
        //inital camera position
        if (followTarget != null)
        {
            newPosition = followTarget.transform.position + new Vector3(0f, 0f, -CameraDistance);
            lastPosition = newPosition;
        }
        else
            newPosition = lastPosition;

        //add mouse positioning
        newPosition += cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraDistance));
        newPosition = new Vector3(newPosition.x / 2, newPosition.y / 2, -CameraDistance);
        transform.position = newPosition;

        //clamp
        float x = Mathf.Clamp(transform.position.x, ws.cameraBoundaries[0], ws.cameraBoundaries[1]);
        float y = Mathf.Clamp(transform.position.y, ws.cameraBoundaries[2], ws.cameraBoundaries[3]);
        transform.position = new Vector3(x, y, transform.position.z);
        
        if (shakeFactor >= 0.0001f)
        {
            transform.position += (Vector3)(Random.insideUnitCircle * shakeFactor * shakeMultiplier);
        }
        shakeFactor *= shakeDecayFactor;
        
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

    public void AddNoise(float a) {
		shakeFactor += a * 0.1f;
	}

    IEnumerator ResetScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while(!load.isDone)
        {
            yield return null;
        }
    }
}
