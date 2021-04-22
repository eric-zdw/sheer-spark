using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenWeapon2CollisionExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
