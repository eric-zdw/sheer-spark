using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowProjectile2 : Projectile {

    public GameObject explosion;
    public GameObject hitbox;

    private Camera cam;
    private Vector3 mousePosition;
    private float radius;

    private float projectileSpeedIncrease = 20f;
    private int layermask = ~((1 << 9) | (1 << 13) | (1 << 8) | (1 << 14) | (1 << 18) | (1 << 21));

    private GameObject player;

    //private float angle1;
    //private float angle2;
    
    // Use this for initialization
    void Start() {
        projectileSpeed = 60f;
        lifeTime = 5f;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 rawmousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        mousePosition = player.transform.position + Vector3.Normalize(rawmousePosition - player.transform.position) * 10000f;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
        {
            CheckLinecastCollision();
            //Propogate();
            lifeTime -= Time.deltaTime;
        }

        //Vector3 rawmousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraFollow.CameraDistance));
        //mousePosition = player.transform.position + Vector3.Normalize(rawmousePosition - player.transform.position) * 1000f;


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
            Vector3 newR = transform.rotation.eulerAngles - new Vector3(0, 0,  (360 - (Mathf.Abs(leftAngle - rightAngle))) * Time.deltaTime * 4f);
            transform.rotation = Quaternion.Euler(newR);
        }
        else
        {
            Vector3 newR = transform.rotation.eulerAngles + new Vector3(0, 0, (360 - (Mathf.Abs(leftAngle - rightAngle))) * Time.deltaTime * 4f);
            transform.rotation = Quaternion.Euler(newR);
        }
        //print(360 - (Mathf.Abs(leftAngle - rightAngle)));
        
        /*
        if (projectileSpeed < 120f) {
            projectileSpeed += projectileSpeedIncrease * Time.deltaTime;
            projectileSpeedIncrease *= 1.18f;
        }
        else {
            projectileSpeed = 120f;
        }
        */
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layermask) {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Explode();
            }
            else if (!(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Powerup")))
            {
                Explode();
            }
        }
    }

    void CheckLinecastCollision() {
        RaycastHit info;
        if (Physics.SphereCast(transform.position, 0.25f, transform.right, out info, projectileSpeed * Time.deltaTime, layermask)) {
            transform.position = info.point;
            if (info.collider.gameObject.CompareTag("Enemy")) {
                //info.collider.gameObject.GetComponent<Enemy>().getDamage(damage);
                Camera.main.GetComponent<CameraFollow>().AddNoise(1.5f);
            }
            Explode();
        }
        else
            transform.position += transform.right * projectileSpeed * Time.deltaTime;
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
		Camera.main.GetComponent<CameraFollow>().AddNoise(1f);
        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        exp.transform.localScale = new Vector3(radius * 0.5f, radius * 0.5f, radius * 0.5f);

        OrangeProjectileHitbox hb = Instantiate(hitbox, transform.position, Quaternion.identity).GetComponent<OrangeProjectileHitbox>();
        hb.GetComponent<SphereCollider>().radius = radius;
        hb.setDamage(damage);
        hb.setRadius(radius);               
        hb.printRadius();
        transform.GetChild(0).parent = null;

        Destroy(gameObject);
    }


}
