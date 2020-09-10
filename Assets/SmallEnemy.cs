using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmallEnemy : Enemy
{
    [SerializeField]
    protected float powerupChance = 0f;

    private GameObject healthBar;
    protected int powerupRoll;
    
    private Color enemyColor;
    private Color enemyColorHDR;

    private MeshRenderer mainMesh;
    private MeshRenderer outlineMesh;
    private MeshRenderer seeThroughMesh;
    private MeshRenderer damageFlashMesh;

    private MaterialPropertyBlock mainMPB;
    private MaterialPropertyBlock seeThroughMPB;
    private MaterialPropertyBlock damageFlashMPB;

    protected override void Initialize()
    {
        base.Initialize();
        //create health bar
        healthBar = Instantiate(healthBarPrefab);
        healthBar.GetComponent<HealthBar>().setTarget(gameObject);
        health = maxHealth;

        powerupRoll = Random.Range(0, 6);
        enemyColor = powerupColors[powerupRoll];
        enemyColorHDR = powerupColorsHDR[powerupRoll];
        //todo: change MatBlock to color

        mainMesh = GetComponent<MeshRenderer>();
        mainMPB = new MaterialPropertyBlock();
        mainMPB.SetColor("Color_3238E920", enemyColorHDR);
        mainMesh.SetPropertyBlock(mainMPB);
        
        outlines[powerupRoll].ApplyTo(outlineMesh.material);

        seeThroughMesh = transform.GetChild(1).GetComponent<MeshRenderer>();
        seeThroughMPB = new MaterialPropertyBlock();
        Color seeThroughColor = new Color(enemyColor.r, enemyColor.g, enemyColor.b, 0.015625f);
        seeThroughMPB.SetColor("_TintColor", seeThroughColor);
        outlineMesh.SetPropertyBlock(seeThroughMPB);

        damageFlashMesh = transform.GetChild(2).GetComponent<MeshRenderer>();
        damageFlashMPB = new MaterialPropertyBlock();
    }

    public override void getDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            DefeatRoutine();
        }
        else {
            StartCoroutine(FlashWhite());
        }
    }

    public override void setHealth(float h)
    {
        health = h;
        if (health <= 0) {
            DefeatRoutine();
        }
    }

    protected void DefeatRoutine() {
        Camera.main.GetComponent<CameraFollow>().AddNoise(5f);
        Instantiate(PowerupManager.powerups[powerupRoll], transform.position, Quaternion.identity);
		Instantiate(deathExplosions[powerupRoll], transform.position, transform.rotation);
		Destroy(healthBar);
		Destroy(gameObject);
    }

    IEnumerator FlashWhite() {
		float colorValue = 2f;
		Color newColor = new Color(colorValue, colorValue, colorValue, 1);
		while (colorValue > 0f) {
			print("colorValue: " + colorValue);
			colorValue -= 5f * Time.deltaTime;
			newColor = new Color(colorValue, colorValue, colorValue, 1);
			damageFlashMPB.SetColor("_EmissionColor", newColor);
			damageFlashMesh.SetPropertyBlock(damageFlashMPB);
			yield return new WaitForFixedUpdate();
		}
	}
}
