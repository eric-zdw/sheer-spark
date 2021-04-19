using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MoveRotation(transform.rotation * Quaternion.Euler(Vector3.right * 10f * Time.deltaTime));
    }
}
