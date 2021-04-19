using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRotation : MonoBehaviour
{
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MoveRotation(transform.rotation * Quaternion.Euler(Vector3.forward * speed * Time.deltaTime));
        //transform.rotation *= Quaternion.Euler(Vector3.forward * -5f * Time.deltaTime);
    }
}
