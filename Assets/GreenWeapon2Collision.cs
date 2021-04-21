using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenWeapon2Collision : MonoBehaviour
{
    // Start is called before the first frame update
    List<Collider> enemyCollider;

    public GameObject explosion;
    public float damage;

    void Start()
    {
        /*
        enemyCollider = new List<Collider>();
        enemyCollider.Add(GetComponent<Collider>());
        foreach (Collider c in transform.GetComponentsInChildren<Collider>())
        {
            enemyCollider.Add(c);
        }

        foreach (Collider c in enemyCollider)
        {
            gameObject.AddComponent(Collider)
        }
        */

        GetComponent<SphereCollider>().radius = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        transform.parent.GetComponent<Enemy>().getDamage(damage);
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().getDamage(damage);
        }
        Destroy(gameObject);
    }
}
