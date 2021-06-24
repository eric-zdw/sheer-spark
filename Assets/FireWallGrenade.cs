using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallGrenade : EnemyProjectile
{
	Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		LeapToTarget(WaveSystem.player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void LeapToTarget(Vector3 target)
	{
		bool isInNegativeXDirection = false;

		float g = -Physics.gravity.y;
		float heightOfJump = (target.y - transform.position.y) * 1.5f;

		//undershoot target slightly.
		Vector3 relativeTarget = (target - transform.position) * 0.8f;
		if (relativeTarget.x < 0)
		{
			isInNegativeXDirection = true;
			relativeTarget = new Vector3(-relativeTarget.x, relativeTarget.y, relativeTarget.z);
		}

		float speed = 6f + (1f * Vector3.Distance(transform.position, target));

		float tan1 = (Mathf.Pow(speed, 2) + Mathf.Sqrt(Mathf.Pow(speed, 4) - g * (g * Mathf.Pow(relativeTarget.x, 2) + 2 * relativeTarget.y * Mathf.Pow(speed, 2)))) / (g * relativeTarget.x);
		//float tan2 = (Mathf.Pow(speed, 2) - Mathf.Sqrt(Mathf.Pow(speed, 4) - g * (g * Mathf.Pow(relativeTarget.x, 2) + 2 * relativeTarget.y * Mathf.Pow(speed, 2)))) / (g * relativeTarget.x);
		float launchAngle1 = Mathf.Atan(tan1);
		//float launchAngle2 = Mathf.Atan(tan2);

		//print(launchAngle1 * Mathf.Rad2Deg + ", " + launchAngle2 * Mathf.Rad2Deg);

		Vector3 velocity = new Vector3(Mathf.Cos(launchAngle1), Mathf.Sin(launchAngle1)) * speed;
		if (float.IsNaN(velocity.x) || float.IsNaN(velocity.y))
		{
			Debug.LogError("Attempted jump is not possible with given speed!");
		}
		else
		{
			if (isInNegativeXDirection)
			{
				velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
			}


			rb.velocity = Vector3.zero;
			rb.AddForce(velocity, ForceMode.VelocityChange);
			if (isInNegativeXDirection)
			{
				rb.AddTorque(0f, 0f, 50f, ForceMode.VelocityChange);
			}
			else
			{
				rb.AddTorque(0f, 0f, -50f, ForceMode.VelocityChange);
			}
		}

	}
}
