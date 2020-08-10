using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {
    public GameObject followTarget;
    public static float CameraDistance = 20f;
    Camera cam;

	public float shakeFactor = 0f;
    public float shakeMultiplier = 1f;
    public float shakeDecayFactor;
    private Vector3 lastPosition;

    private float resetTimer = 5f;
    private bool hasRestarted = false;

    public CameraBounds bounds;
    public Vector2 playerScreenPosition;

	// Use this for initialization
	void Start ()
    {
        followTarget = GameObject.Find("Player");
        cam = GetComponent<Camera>();
        bounds = GameObject.Find("CameraBounds").GetComponent<CameraBounds>();
	}
	
	// Update is called once per frame
	void Update ()
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
        Vector3 mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraDistance));
        newPosition = Vector3.Lerp(newPosition, mousePosition, 0.4f);
        newPosition = new Vector3(newPosition.x, newPosition.y, -CameraDistance);
        //newPosition += cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraDistance));
        //newPosition = new Vector3(newPosition.x / 2, newPosition.y / 2, -CameraDistance);
        transform.position = newPosition;

        //clamp
        if (bounds) {
            float x = Mathf.Clamp(transform.position.x, bounds.bottomLeft.x, bounds.topRight.x);
            float y = Mathf.Clamp(transform.position.y, bounds.bottomLeft.y, bounds.topRight.y);
            transform.position = new Vector3(x, y, transform.position.z);
        }
        
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

        playerScreenPosition = cam.WorldToScreenPoint(followTarget.transform.position);
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
