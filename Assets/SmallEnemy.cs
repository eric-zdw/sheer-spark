using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmallEnemy : Enemy
{
    public SmallEnemyScriptableObject smallEnemyData;

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
        healthBar = Instantiate(smallEnemyData.healthBarPrefab);
        healthBar.GetComponent<HealthBar>().setTarget(gameObject);
        health = maxHealth;

        powerupRoll = Random.Range(0, 6);
        enemyColor = smallEnemyData.powerupColors[powerupRoll];
        enemyColorHDR = smallEnemyData.powerupColorsHDR[powerupRoll];
        //todo: change MatBlock to color

        mainMesh = GetComponent<MeshRenderer>();
        mainMPB = new MaterialPropertyBlock();
        mainMPB.SetColor("Color_3238E920", enemyColorHDR);
        mainMesh.SetPropertyBlock(mainMPB);
        
        outlineMesh = transform.GetChild(0).GetComponent<MeshRenderer>();
        outlineMesh.material = smallEnemyData.outlines[powerupRoll];

        seeThroughMesh = transform.GetChild(1).GetComponent<MeshRenderer>();
        seeThroughMesh.material = smallEnemyData.seeThroughMats[powerupRoll];

        damageFlashMesh = transform.GetChild(2).GetComponent<MeshRenderer>();
        damageFlashMPB = new MaterialPropertyBlock();
    }

    public override void getDamage(float damage) {
        //Check health previous to damage calculation to prevent duplicate deaths
        float oldHealth = health;
        if (oldHealth > 0f) {
            health -= damage;
            if (health <= 0f) {
                DefeatRoutine();
            }
            else {
                StartCoroutine(FlashWhite());
            }
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
        Destroy(healthBar);
        Camera.main.GetComponent<CameraFollow>().AddNoise(5f);
        Instantiate(smallEnemyData.powerups[powerupRoll], transform.position, Quaternion.identity);
		Instantiate(smallEnemyData.deathExplosions[powerupRoll], transform.position, transform.rotation);
        ScoreManager.IncreaseScore(Mathf.RoundToInt(scoreValue * ScoreManager.multiplier));
		Destroy(gameObject);
    }

    IEnumerator FlashWhite() {
		float colorValue = 2f;
		Color newColor = new Color(colorValue, colorValue, colorValue, 1);
		while (colorValue > 0f) {
			//print("colorValue: " + colorValue);
			colorValue -= 5f * Time.deltaTime;
			newColor = new Color(colorValue, colorValue, colorValue, 1);
			damageFlashMPB.SetColor("_EmissionColor", newColor);
			damageFlashMesh.SetPropertyBlock(damageFlashMPB);
			yield return new WaitForFixedUpdate();
		}
	}
}
