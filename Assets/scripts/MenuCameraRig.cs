using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraRig : MonoBehaviour {

	public Vector3[] positions;
	public Vector3[] rotations;
	public static int currentPosition = 0;
	public float transitionTime = 2f;
	public float oldtransitionRate = 0.02f;
	private static float timer = 0f;

	private static Vector3 startPosition, endPosition;
	private static Quaternion startRotation, endRotation;

	private float positionVelocity, rotationVelocity;

	// Use this for initialization
	void Start () {
		timer = transitionTime;
		startPosition = transform.position;
		startRotation = transform.rotation;
	}

    private void Update()
    {
		if (timer < transitionTime)
        {
			timer += Time.deltaTime;
			//transform.position = Vector3.Lerp(startPosition, positions[currentPosition], Mathf.SmoothStep(0f, 1f, (timer / transitionTime)));
			//transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(rotations[currentPosition]), Mathf.SmoothStep(0f, 1f, (timer / transitionTime)));
		}
		else
        {
			timer = transitionTime;
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Vector3.Lerp(transform.position, positions[currentPosition], oldtransitionRate);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotations[currentPosition]), oldtransitionRate);
	}

	public static void ChangePosition(int index) {
		currentPosition = index;
		startPosition = Camera.main.transform.position;
		startRotation = Camera.main.transform.rotation;
		timer = 0f;
	}

	public void ChangePositionInstance(int index) {
		ChangePosition(index);
	}
}
