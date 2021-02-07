using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {
    public GameObject followTarget;
    public static float CameraDistance = 24f;
    Camera cam;

	public float shakeFactor = 0f;
    public float shakeMultiplier = 1f;
    public float shakeDecayFactor;
    private Vector3 lastPosition;

    private float resetTimer = 5f;
    private bool hasRestarted = false;

    public CameraBounds bounds;
    public Vector2 playerScreenPosition;

    private bool inIntro = true;
    public Vector3 startPosition;

    public bool isReversed = false;

    public float mousePositionMagnitude;
    private Vector3 cameraVelocity;
    public bool smoothDampEnabled = false;
    public float smoothDampValue = 0.05f;

	// Use this for initialization
	void Start ()
    {
        followTarget = GameObject.Find("Player");
        cam = GetComponent<Camera>();
        //bounds = GameObject.Find("CameraBounds").GetComponent<CameraBounds>();

        StartCoroutine(StageIntroRoutine());
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
        if (!inIntro) {
            Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 mousePosRelativeToCenter = new Vector2(Input.mousePosition.x - center.x, Input.mousePosition.y - center.y);
            //normalize mousePosRelativeToCenter
            mousePosRelativeToCenter /= new Vector2(Screen.width, Screen.height);
            
            if (isReversed) {
                mousePosRelativeToCenter.x *= -1;
            }

            newPosition += (Vector3)(mousePosRelativeToCenter * mousePositionMagnitude);

            //old mouse positioning code, has lerp drift
            /*
            Vector3 mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(CameraDistance)));
            newPosition = Vector3.Lerp(newPosition, mousePosition, 0.4f);
            newPosition = new Vector3(newPosition.x, newPosition.y, followTarget.transform.position.z - CameraDistance);
            */
            
            if (smoothDampEnabled) {
                transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref cameraVelocity, smoothDampValue);
            }
            else {
                transform.position = newPosition;
            }
            
        }
        //newPosition += cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraDistance));
        //newPosition = new Vector3(newPosition.x / 2, newPosition.y / 2, -CameraDistance);

        //add reversed camera
        if (isReversed) {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        

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

    IEnumerator StageIntroRoutine() {
        inIntro = true;
        float lerp = 0f;
        float t = 0f;

        while (lerp < 0.999f) {
            Vector3 mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraDistance));
            Vector3 newPosition = followTarget.transform.position + new Vector3(0f, 0f, -CameraDistance);
            newPosition = Vector3.Lerp(newPosition, mousePosition, 0.4f);
            newPosition = new Vector3(newPosition.x, newPosition.y, -CameraDistance);
            transform.position = Vector3.Lerp(startPosition, newPosition, lerp);

            t += Time.deltaTime * 0.15f;
            lerp = Mathf.SmoothStep(0f, 1f, t);
            yield return new WaitForEndOfFrame();
        }

        inIntro = false;
    }

    //calculate new position immediately and skip lerping; mainly used for teleporting
    public void SnapToNewPosition() {
        Vector3 newPosition;
        newPosition = followTarget.transform.position + new Vector3(0f, 0f, -CameraDistance);

        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosRelativeToCenter = new Vector2(Input.mousePosition.x - center.x, Input.mousePosition.y - center.y);
        //normalize mousePosRelativeToCenter
        mousePosRelativeToCenter /= new Vector2(Screen.width, Screen.height);
        
        if (isReversed) {
            mousePosRelativeToCenter.x *= -1;
        }

        newPosition += (Vector3)(mousePosRelativeToCenter * mousePositionMagnitude);
        transform.position = newPosition;
    }
}
