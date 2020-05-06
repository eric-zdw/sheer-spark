using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBossPiece : Enemy {

	public OrangeBossHead head;

	private MaterialPropertyBlock damageMatBlock;
	private MeshRenderer damageFlash;

	// Use this for initialization
	void Start () {
		damageFlash = transform.GetChild(2).GetComponent<MeshRenderer>();
		damageMatBlock = new MaterialPropertyBlock();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void getDamage(float damage)
    {
        head.getDamageNoFlash(damage * 0.12f);
		StartCoroutine(FlashWhite());
    }

	private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
        }
    }

	IEnumerator FlashWhite() {
		float colorValue = 2f;
		Color newColor = new Color(colorValue, colorValue, colorValue, 1);
		while (colorValue > 0f) {
			colorValue -= 5f * Time.deltaTime;
			newColor = new Color(colorValue, colorValue, colorValue, 1);
			damageMatBlock.SetColor("_EmissionColor", newColor);
			damageFlash.SetPropertyBlock(damageMatBlock);
			yield return new WaitForFixedUpdate();
		}
	}
}
