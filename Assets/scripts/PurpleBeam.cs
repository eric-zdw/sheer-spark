using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBeam : MonoBehaviour {

    private ParticleSystem beam;
    private float timer = 0.2f;
    private float decayRate = 0f;
    private bool decaySet = false;
    private float damage = 0f;

    public GameObject explosion;
    public GameObject explosion2;
    public GameObject explosion3;
    private CapsuleCollider collider;
    private AudioSource beamSound;
    

	// Use this for initialization
	void Start () {
        collider = GetComponent<CapsuleCollider>();
        beam = GetComponent<ParticleSystem>();
        Destroy(collider, 0.15f);
        beamSound = GetComponent<AudioSource>();
        Camera.main.GetComponent<CameraFollow>().AddNoise(beam.startSize * 20f);
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0f && beam.startSize > 0)
        {
            if (decaySet == false)
            {
                decayRate = beam.startSize * 8f;
                decaySet = true;
            }
            beam.startSize -= decayRate * Time.deltaTime;
        }
        if (beam.startSize <= 0f)
        {
            Destroy(gameObject, 2f);
            beam.startSize = 0f;
        }
	}

    public void setSize(float s)
    {
        beam.startSize = s;
        beamSound.pitch = 3 - s;
    }

    public void setDamage(float d)
    {
        damage = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            print("damage dealt: " + damage);
            other.GetComponent<Enemy>().getDamage(damage);
            GameObject exp = Instantiate(explosion, collider.ClosestPointOnBounds(other.transform.position), Quaternion.identity);
            GameObject exp2 = Instantiate(explosion2, collider.ClosestPointOnBounds(other.transform.position), Quaternion.identity);
            GameObject exp3 = Instantiate(explosion3, collider.ClosestPointOnBounds(other.transform.position), Quaternion.identity);
            float expSize = (beam.startSize * 0.5f) + 0.5f;
            exp.transform.localScale = new Vector3(expSize, expSize, expSize);
            exp2.transform.localScale = new Vector3(expSize, expSize, expSize);
            exp3.transform.localScale = new Vector3(expSize, expSize, expSize);
        }
    }
}
