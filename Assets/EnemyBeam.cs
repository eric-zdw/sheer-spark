using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour {

    private ParticleSystem beam;
    private float timer = 0.2f;
    private float decayRate = 0f;
    private bool decaySet = false;
    private float damage = 0f;
    
    private CapsuleCollider collider;
    private AudioSource beamSound;

    private GameObject cam;

	// Use this for initialization
	void Start () {
        collider = GetComponent<CapsuleCollider>();
        beam = GetComponent<ParticleSystem>();
        Destroy(collider, 0.15f);
        beamSound = GetComponent<AudioSource>();

        beam.startSize = 0.5f;
        collider.radius = beam.startSize / 2f;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        print(cam.name);
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
        if (other.tag == "Player")
        {
            print("damage dealt: " + damage);
            other.GetComponent<PlayerBehaviour>().takeDamage(1);
        }
    }
}
