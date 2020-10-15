using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemySensor : MonoBehaviour
{
    public FastEnemy fastEnemy;
    float dodgeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dodgeTimer > 0f) {
            dodgeTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile") && dodgeTimer <= 0f) {
            Vector3 angle = Vector3.Normalize(other.transform.position - transform.position);
            fastEnemy.Dodge(angle);
            dodgeTimer = 0.6f;
        }
    }
}
