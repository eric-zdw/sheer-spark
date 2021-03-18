using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrystalGlow : MonoBehaviour {

	private MeshRenderer meshRenderer;
	private MaterialPropertyBlock matBlock;
	private float currentEmission;
	private int direction;

	private float minimumGlow = 2f;
	private float maximumGlow = 6f;
	private float glowSpeed = 1.5f;
	private float realGlowSpeed = 0f;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
		matBlock = new MaterialPropertyBlock();
		currentEmission = Random.Range(minimumGlow, maximumGlow);
		direction = Random.Range(0, 1);
		realGlowSpeed = glowSpeed + Random.Range(realGlowSpeed *= 0.5f, realGlowSpeed *= 1.5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (direction == 0) {
			currentEmission -= Time.deltaTime * realGlowSpeed;
			matBlock.SetColor("_EmissionColor", new Color(.375f, .5f, 1f) * currentEmission);
		}
		else {
			currentEmission += Time.deltaTime * realGlowSpeed;
			matBlock.SetColor("_EmissionColor", new Color(.375f, .5f, 1f) * currentEmission);
		}

		if (currentEmission <= minimumGlow) direction = 1;
		else if (currentEmission >= maximumGlow) direction = 0;
		
		meshRenderer.SetPropertyBlock(matBlock);
	}
}
