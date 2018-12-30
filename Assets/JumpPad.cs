using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    public float boost;
    public float boostCooldown = 0.1f;
    private float currentBoostCooldown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (currentBoostCooldown > 0f)
        {
            currentBoostCooldown -= Time.deltaTime;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && currentBoostCooldown <= 0f)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - boost, rb.velocity.z);
            currentBoostCooldown = boostCooldown;
        }
    }
}
