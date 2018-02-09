using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowProjectile : Projectile {

    public GameObject explosion;
    public GameObject explosion2;

    private Camera cam;
    private Vector3 mousePosition;
    
    // Use this for initialization
    void Start() {
        projectileSpeed = 25f;
        lifeTime = 4f;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            Propogate();
            lifeTime -= Time.deltaTime;
        }

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9f));
        Debug.DrawLine(transform.position, transform.position + transform.right, Color.red, 0.1f);
        Debug.DrawLine(transform.position, mousePosition, Color.white, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();
            other.gameObject.GetComponent<Enemy>().getDamage(damage);
            print("real damage: " + damage);
        }
        else if (!other.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void setDamage(float d)
    {
        damage = d;
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Instantiate(explosion2, transform.position, transform.rotation);
        Destroy(gameObject);
    }


}
