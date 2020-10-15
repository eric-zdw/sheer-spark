using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBeam : MonoBehaviour {

    private ParticleSystem beam;
    private ParticleSystem.MainModule main;
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
        main = beam.main;
        Destroy(collider, 0.15f);
        beamSound = GetComponent<AudioSource>();
        Camera.main.GetComponent<CameraFollow>().AddNoise(main.startSize.constant * 20f);
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0f && main.startSize.constant > 0)
        {
            if (decaySet == false)
            {
                decayRate = main.startSize.constant * 8f;
                decaySet = true;
            }
            main.startSize = new ParticleSystem.MinMaxCurve(main.startSize.constant - decayRate * Time.deltaTime);
        }
        if (main.startSize.constant <= 0f)
        {
            Destroy(gameObject, 2f);
            main.startSize = 0f;
        }
	}

    public void setSize(float s)
    {
        main.startSize = new ParticleSystem.MinMaxCurve(s);
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
            float expSize = (main.startSize.constant * 0.5f) + 0.5f;
            exp.transform.localScale = new Vector3(expSize, expSize, expSize);
            exp2.transform.localScale = new Vector3(expSize, expSize, expSize);
            exp3.transform.localScale = new Vector3(expSize, expSize, expSize);
        }
    }
}
