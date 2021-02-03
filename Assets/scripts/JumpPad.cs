using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    public Vector3 boost;
    public float boostCooldown = 0.1f;
    private float currentBoostCooldown;

    //display HDR
    [ColorUsageAttribute(true,true)]
    public Color particleColor;
    [ColorUsageAttribute(true,true)]
    public Color fresnelColor;
    [ColorUsageAttribute(true,true)]
    public Color meshColor;
    public GameObject boostPrefab;

    private MaterialPropertyBlock particleMpb, meshMpb;
    private ParticleSystemRenderer renderer;
    private MeshRenderer mesh;

	// Use this for initialization
	void Start () {
		particleMpb = new MaterialPropertyBlock();
        meshMpb = new MaterialPropertyBlock();
        renderer = GetComponent<ParticleSystemRenderer>();
        mesh = GetComponent<MeshRenderer>();

        particleMpb.SetColor("_Color", particleColor);
        particleMpb.SetColor("_EmissionColor", particleColor);
        renderer.SetPropertyBlock(particleMpb);

        meshMpb.SetColor("Color_3238E920", fresnelColor);
        meshMpb.SetColor("Color_34C5F63F", meshColor);
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

            Instantiate(boostPrefab, transform.position, transform.rotation);
            StartCoroutine(BoostGlow());
        }
    }

    IEnumerator BoostGlow() {
        float duration = 0.8f;
        while (duration > 0) {
            meshMpb.SetColor("Color_3238E920", fresnelColor + (fresnelColor * 5f * duration));
            mesh.SetPropertyBlock(meshMpb);
            duration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        meshMpb.SetColor("Color_3238E920", fresnelColor);
        mesh.SetPropertyBlock(meshMpb);
    }
}
