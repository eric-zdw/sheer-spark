using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEnemy : SmallEnemy
{
    public GameObject enemy;
    public GameObject spheres;
    public GameObject grenade;
    public float hoverStrength = 4500f;
    public float hoverRange = 8f;
    public float rotationStrength = 5f;
    public float cooldownTime = 4f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(LobGrenadeRoutine());
        StartCoroutine(SelfOrientRoutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, hoverRange, 1 << 17))
        {
            float d = hit.distance;
            rb.AddForce(Vector3.up * 1000f * Time.deltaTime);
            rb.AddForce(Vector3.up * hoverStrength * ((hoverRange - d) / hoverRange) * Time.deltaTime);
        }
    }

    protected override void DefeatRoutine()
    {
        Destroy(healthBar);
        Camera.main.GetComponent<CameraFollow>().AddNoise(5f);
        Instantiate(smallEnemyData.powerups[powerupRoll], transform.position, Quaternion.identity);
        Instantiate(smallEnemyData.deathExplosions[powerupRoll], transform.position, transform.rotation);
        ScoreManager.IncreaseScore(scoreValue);
        Destroy(enemy);
    }

    public IEnumerator LobGrenadeRoutine()
    {
        float cooldown = 0f;

        while (true)
        {
            if (!Physics.Linecast(transform.position, WaveSystem.player.transform.position, 1 << 17) && cooldown <= 0)
            {
                cooldown += cooldownTime;
                Vector3 spherePos = spheres.transform.GetChild(0).transform.position;
                Instantiate(grenade, new Vector3(spherePos.x, spherePos.y, transform.position.z), Quaternion.identity);
            }
            else
            {
                cooldown -= Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator SelfOrientRoutine()
    {
        while (true)
        {
            float rotationDifference = transform.rotation.eulerAngles.z;
            float strength;

            //counterclockwise from 0f
            if (rotationDifference <= 180f)
            {
                //distance away from 0f
                strength = Mathf.Abs(rotationDifference);
                //print(strength);

                //move clockwise
                rb.AddTorque(Vector3.forward * -rotationStrength * strength * Time.deltaTime);
            }
            //clockwise from 0f
            else if (rotationDifference > 180f)
            {
                //distance away from 360f
                strength = Mathf.Abs(rotationDifference - 360f);
                print(strength);

                //move counterclockwise
                rb.AddTorque(Vector3.forward * rotationStrength * strength * Time.deltaTime);
            }

            
            //rb.AddTorque(Vector3.forward * 1f * rotationDifference * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
