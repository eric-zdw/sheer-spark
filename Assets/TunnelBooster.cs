using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBooster : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float magnitude = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        if (other.GetComponent<Rigidbody>() != null) {
            other.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(direction) * magnitude * Time.deltaTime, ForceMode.Acceleration);
            if (other.GetComponent<Rigidbody>().useGravity == true)
            {
                other.GetComponent<Rigidbody>().AddForce(-Physics.gravity * 40f * Time.deltaTime, ForceMode.Acceleration);
            }
        }
    }
}
