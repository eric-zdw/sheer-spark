using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class JumpPad : MonoBehaviour {

    public Vector3 boost;
    public float boostCooldown = 0.1f;
    private float currentBoostCooldown;

    //display HDR
    [ColorUsageAttribute(true,true)]
    public Color particleColor;
    [ColorUsageAttribute(true,true)]
    public Color glowColor;
    [ColorUsageAttribute(true,true)]
    public Color fresnelColor;
    [ColorUsageAttribute(true,true)]
    public Color meshColor;
    public GameObject boostPrefab;
    public GameObject glowObject;

    private MaterialPropertyBlock particleMpb, meshMpb, glowMpb, boostMpb;
    private ParticleSystemRenderer renderer;
    private ParticleSystemRenderer glowRenderer;
    private MeshRenderer mesh;

    private void InitializeVisuals() {
		particleMpb = new MaterialPropertyBlock();
        meshMpb = new MaterialPropertyBlock();
        glowMpb = new MaterialPropertyBlock();
        boostMpb = new MaterialPropertyBlock();
        renderer = GetComponent<ParticleSystemRenderer>();
        glowRenderer = glowObject.GetComponent<ParticleSystemRenderer>();
        mesh = GetComponent<MeshRenderer>();

        particleMpb.SetColor("_Color", particleColor);
        particleMpb.SetColor("_EmissionColor", particleColor);
        renderer.SetPropertyBlock(particleMpb);

        glowMpb.SetColor("_Color", glowColor);
        glowMpb.SetColor("_EmissionColor", glowColor);
        glowRenderer.SetPropertyBlock(glowMpb);

        meshMpb.SetColor("Color_3238E920", fresnelColor);
        meshMpb.SetColor("Color_34C5F63F", meshColor);
        mesh.SetPropertyBlock(meshMpb);

        boostMpb.SetColor("_Color", glowColor);
        boostMpb.SetColor("_EmissionColor", glowColor);        
    }

	// Use this for initialization
	void Start () {
		InitializeVisuals();
	}

    void OnValidate() {
		//InitializeVisuals();
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

            GameObject boostObject = Instantiate(boostPrefab, transform.position, transform.rotation);
            ParticleSystemRenderer boostParticles = boostObject.GetComponent<ParticleSystemRenderer>();
            boostParticles.SetPropertyBlock(boostMpb);
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
