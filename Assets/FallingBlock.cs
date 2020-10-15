using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    Rigidbody rb;
    private float speed = -7f;
    private float realSpeed;

    private float rotationSpeed = 20f;
    private float realRotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        realSpeed = Random.Range(speed - 3f, speed + 3f);

        realRotationSpeed = Random.Range(-rotationSpeed, rotationSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + new Vector3(0f, realSpeed * Time.deltaTime, 0f));
        rb.MoveRotation(rb.transform.rotation * Quaternion.Euler(0f, 0f, realRotationSpeed * Time.deltaTime));
        
    }
}
