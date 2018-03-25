using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowProjectile : Projectile {

    public GameObject explosion;
    public GameObject hitbox;

    private Camera cam;
    private CameraFollow camscript;
    private Vector3 mousePosition;
    private float radius;

    //private float angle1;
    //private float angle2;
    
    // Use this for initialization
    void Start() {
        projectileSpeed = 25f;
        lifeTime = 3.5f;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camscript = cam.GetComponent<CameraFollow>();
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

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 14f));
        Debug.DrawLine(transform.position, transform.position + transform.right, Color.red, 0.1f);
        Debug.DrawLine(transform.position, mousePosition, Color.white, 0.1f);


        /*
        Quaternion newE = (Quaternion.LookRotation(mousePosition - transform.position) * Quaternion.Euler(0, -90, 0));
        //float newZ = Mathf.Lerp(transform.rotation.eulerAngles.z, newR.eulerAngles.z, 0.5f * Time.deltaTime);
        //Vector3 curRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Slerp(transform.rotation, newE, 5f * Time.deltaTime);
        */
        
        float angle1 = Vector3.Angle(Vector3.right, mousePosition - transform.position);
        if (mousePosition.y < transform.position.y)
        {
            angle1 = -(angle1 - 180) + 180;
        }
        float angle2 = Vector3.Angle(Vector3.right, transform.right);
        if ((transform.position + transform.right).y < transform.position.y)
        {
            angle2 = -(angle2 - 180) + 180;
        }
        
        float leftAngle;
        float rightAngle;
        if (angle2 > angle1)
            leftAngle = angle2 - angle1;
        else if (angle2 < angle1)
            leftAngle = angle2 + (360f - angle1);
        else
            leftAngle = 0f;
        rightAngle = 360 - leftAngle;
        

        if (leftAngle < rightAngle)
        {
            Vector3 newR = transform.rotation.eulerAngles - new Vector3(0, 0,  (360 - (Mathf.Abs(leftAngle - rightAngle))) * Time.deltaTime * 2.5f);
            transform.rotation = Quaternion.Euler(newR);
        }
        else
        {
            Vector3 newR = transform.rotation.eulerAngles + new Vector3(0, 0, (360 - (Mathf.Abs(leftAngle - rightAngle))) * Time.deltaTime * 2.5f);
            transform.rotation = Quaternion.Euler(newR);
        }
        print(360 - (Mathf.Abs(leftAngle - rightAngle)));
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();
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

    public void setRadius(float r)
    {
        radius = r;
    }

    void Explode()
    {
        camscript.addShake(0.06f);
        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        exp.transform.localScale = new Vector3(radius * 0.5f, radius * 0.5f, radius * 0.5f);

        OrangeProjectileHitbox hb = Instantiate(hitbox, transform.position, Quaternion.identity).GetComponent<OrangeProjectileHitbox>();
        hb.GetComponent<SphereCollider>().radius = radius;
        hb.setDamage(damage);
        hb.setRadius(radius / 0.25f);                //minimum damage is 1-x%
        hb.printRadius();

        Destroy(gameObject);
    }


}
