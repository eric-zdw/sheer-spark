using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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

    private Rigidbody rb;

    protected bool isDelayedDeath = false;

    protected override void Initialize()
    {
        base.Initialize();
        //create health bar
        healthBar = Instantiate(smallEnemyData.healthBarPrefab);
        healthBar.GetComponent<HealthBar>().setTarget(gameObject);
        health = maxHealth;

        rb = GetComponent<Rigidbody>();

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
        if (oldHealth > 0f && !isDelayedDeath) {
            health -= damage;
            if (health <= 0f) {
                DefeatRoutine();
                //StartCoroutine(DelayedDeathRoutine());
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
        ScoreManager.IncreaseScore(scoreValue);
		Destroy(gameObject);
    }

	private IEnumerator DelayedDeathRoutine() {
        isDelayedDeath = true;
		float duration = 2.5f;
		Instantiate(smallEnemyData.delayedDeathPrefab, transform.position, Quaternion.identity);
		rb.AddForce(Vector3.up * 1000f);
        rb.useGravity = true;
		while (duration > 0f) {
			duration -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
        DefeatRoutine();
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
