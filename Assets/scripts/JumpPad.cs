using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    public Vector3 boost;
    public float boostCooldown = 0.1f;
    private float currentBoostCooldown;
    public Color particleColor;
    public Color meshColor;

    private MaterialPropertyBlock particleMpb, meshMpb;
    private ParticleSystemRenderer renderer;
    private MeshRenderer mesh;

	// Use this for initialization
	void Start () {
		particleMpb = new MaterialPropertyBlock();
        meshMpb = new MaterialPropertyBlock();
        renderer = GetComponent<ParticleSystemRenderer>();
        mesh = GetComponent<MeshRenderer>();

        particleMpb.SetColor("_TintColor", particleColor);
        renderer.SetPropertyBlock(particleMpb);

        meshMpb.SetColor("_TintColor", meshColor);
        mesh.SetPropertyBlock(meshMpb);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentBoostCooldown > 0f)
        {
            currentBoostCooldown -= Time.deltaTime;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && currentBoostCooldown <= 0f)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.velocity = boost;
            currentBoostCooldown = boostCooldown;
        }
    }
}
