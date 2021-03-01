using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testjumping : MonoBehaviour
{
    float leapDuration = 10f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LeapRoutine(new Vector3(-20, 10, 0)));
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

	IEnumerator LeapRoutine(Vector3 target) {
		
		//Leap can be interrupted by damage or physics collision.
		bool wasInterrupted = false;
		bool touchedGround = false;

		float g = -Physics.gravity.y;
		float heightOfJump = (target.y - transform.position.y) * 1.5f;

		Vector3 relativeTarget = target - transform.position;

		float speed = 60f;

		float tan1 = (Mathf.Pow(speed, 2) + Mathf.Sqrt(Mathf.Pow(speed, 4) - g * (g * Mathf.Pow(relativeTarget.x, 2) + 2 * relativeTarget.y * Mathf.Pow(speed, 2)))) / (g * relativeTarget.x);
        //float tan2 = (Mathf.Pow(speed, 2) - Mathf.Sqrt(Mathf.Pow(speed, 4) - g * (g * Mathf.Pow(relativeTarget.x, 2) + 2 * relativeTarget.y * Mathf.Pow(speed, 2)))) / (g * relativeTarget.x);
		float launchAngle1 = Mathf.Atan(tan1);
        //float launchAngle2 = Mathf.Atan(tan2);

        //print(launchAngle1 * Mathf.Rad2Deg + ", " + launchAngle2 * Mathf.Rad2Deg);

        Vector3 velocity = new Vector3(Mathf.Cos(launchAngle1), Mathf.Sin(launchAngle1)) * speed;
        print("velocity: " + velocity);

		GetComponent<Rigidbody>().isKinematic = false;

		float x, y;
		float leapTime = 0f;

		while (transform.position != target) {
            transform.position += velocity * Time.deltaTime;
            velocity += Physics.gravity * Time.deltaTime;
            yield return new WaitForFixedUpdate();
		}

        yield return new WaitForEndOfFrame();
	}

    IEnumerator LeapRoutine2(Vector3 target) {
        float g = -Physics.gravity.y;
        float speed = 50f;

        Vector2 relativeTarget = target - transform.position;
        print(relativeTarget);

        float tan = (Mathf.Pow(speed, 2) / (g * relativeTarget.x)) 
                    + Mathf.Sqrt((Mathf.Pow(speed, 2) * (Mathf.Pow(speed, 2) - 2 * g * relativeTarget.y)) / (Mathf.Pow(g, 2) * Mathf.Pow(relativeTarget.x, 2)) - 1);
        
        float angle = Mathf.Atan(tan);
        print(angle * Mathf.Rad2Deg);

        yield return new WaitForEndOfFrame();
    }
}
