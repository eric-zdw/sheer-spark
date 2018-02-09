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
    private CapsuleCollider collider;

	// Use this for initialization
	void Start () {
        collider = GetComponent<CapsuleCollider>();
        beam = GetComponent<ParticleSystem>();
        Destroy(collider, 0.15f);
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
            Destroy(gameObject, 0.5f);
            beam.startSize = 0f;
        }
	}

    public void setSize(float s)
    {
        beam.startSize = s;
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
            float expSize = (beam.startSize * 0.5f) + 0.5f;
            exp.transform.localScale = new Vector3(expSize, expSize, expSize);
        }
    }
}
