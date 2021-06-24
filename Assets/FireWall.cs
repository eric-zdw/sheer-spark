using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : MonoBehaviour
{

    public BoxCollider trigger;
    public bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnableTrigger());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnableTrigger()
    {
        yield return new WaitForSeconds(1.2f);
        trigger.enabled = true;
        yield return new WaitForSeconds(0.4f);
        trigger.enabled = false;
        yield return new WaitForSeconds(2.4f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !hit)
        {
            other.GetComponent<PlayerBehaviour>().takeDamage(1);
            hit = true;
        }
    }
}
