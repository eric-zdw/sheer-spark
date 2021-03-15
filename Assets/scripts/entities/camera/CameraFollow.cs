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
    public float shakeDecayFactor = 0.95f;
    private Vector3 lastPosition;

    private bool hasRestarted = false;

    public CameraBounds bounds;
    public Vector2 playerScreenPosition;

    private bool inIntro = true;
    private bool introStarted = false;
    public Vector3 startPosition;

    public bool isReversed = false;

    public float mousePositionMagnitude;
    private Vector3 cameraVelocity;
    public bool smoothDampEnabled = false;
    public float smoothDampValue = 0.05f;

    public const float shakeReductionInterval = 0.02f;
    private float shakeTimer = 0f;

	// Use this for initialization
	void Start ()
    {
        //followTarget = GameObject.Find("Player");
        //bounds = GameObject.Find("CameraBounds").GetComponent<CameraBounds>();
        cam = GetComponent<Camera>();
        shakeTimer = shakeReductionInterval;
        transform.position = startPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {

        Vector3 newPosition;
        //inital camera position
        if (followTarget != null)
        {
            if (!introStarted)
            {
                introStarted = true;
                StartCoroutine(StageIntroRoutine());
            }
            newPosition = followTarget.transform.position + new Vector3(0f, 0f, -CameraDistance);
            lastPosition = newPosition;

            playerScreenPosition = cam.WorldToScreenPoint(followTarget.transform.position);
        }
        else
        {
            newPosition = lastPosition;
        }

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
        
        //reduce shake factor
        //reduces factor in intervals of 0.02 seconds. Reduces multiple times if deltaTime passes one interval.
        shakeTimer -= Time.unscaledDeltaTime;
        while (shakeTimer < 0f) {
            shakeFactor *= shakeDecayFactor;
            shakeTimer += shakeReductionInterval;
        }
        if (shakeFactor < 0.0001f) {
            shakeFactor = 0f;
        }

        transform.position += (Vector3)(Random.insideUnitCircle * shakeFactor * shakeMultiplier);

        
    }

    public void AddNoise(float a) {
		shakeFactor += a * 0.1f;
	}

    public void Reset()
    {
        print("test");
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

            t += Time.deltaTime * 0.25f;
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
