using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossEnemy : Enemy {
	public float newHealth;

	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

    public float YLimit;
    private int powerupRoll;

	private NoiseManager noiseManager;

	private MaterialPropertyBlock damageMatBlock;

	private int numberOfAttacks = 6;
	private int numberOfAttacksPhaseTwo = 2;

	private enum BossAttack {RedSpread, OrangeMines, YellowMissiles, GreenVortex, BlueTornado, PurpleBeams};
	private enum BossAttackPhaseTwo {WhiteLaserColumns, WhiteLaserSpinning};

	private UnityEngine.UI.Image healthBarLeft;
	private UnityEngine.UI.Image healthBarRight;

    void Start()
	{
		maxHealth = newHealth;
		health = maxHealth;

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
		noiseManager = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<NoiseManager>();

		powerupRoll = Random.Range(0, 6);

		damageMatBlock = new MaterialPropertyBlock();
		StartCoroutine(BossAttackRoutine());
		ActivateHealthBar();
    }

	void FixedUpdate()
	{
		if (health <= 0)
		{
			Explode();
		}

		rb.AddForce(Vector3.Normalize(player.transform.position - transform.position) * 1800f * Time.deltaTime);

		//rb.AddForce(new Vector3(-rb.velocity.x, 0, 0) * 2f * Time.deltaTime);
		//rb.AddForce(new Vector3(0, -rb.velocity.y, 0) * 80f * Time.deltaTime);
	    
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
            Explode();
        }
    }

    void Explode()
	{
		noiseManager.AddNoise(10f);
		Destroy(gameObject);
	}

	public override void getDamage(float damage)
    {
        health -= damage;
		StartCoroutine(FlashWhite());
    }

	IEnumerator FlashWhite() {
		float colorValue = 2f;
		Color newColor = new Color(colorValue, colorValue, colorValue, 1);
		while (colorValue > 0f) {
			colorValue -= 5f * Time.deltaTime;
			newColor = new Color(colorValue, colorValue, colorValue, 1);
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator BossAttackRoutine() {
		while (true) {
			//Wait period
			yield return new WaitForSeconds(5f);

			//Attack period
			BossAttack chooseAttack = (BossAttack)Random.Range(1, numberOfAttacks+1);

			if (chooseAttack == BossAttack.RedSpread) {

			}
			
		}
	}

	private void ActivateHealthBar() {
		GameObject.Find("BossHealthPanel").GetComponent<UnityEngine.UI.Image>().enabled = true;
		GameObject.Find("BossTitle").GetComponent<UnityEngine.UI.Text>().enabled = true;
		healthBarLeft = GameObject.Find("BossHealthBarLeft").GetComponent<UnityEngine.UI.Image>();
		healthBarLeft.enabled = true;
		healthBarRight = GameObject.Find("BossHealthBarRight").GetComponent<UnityEngine.UI.Image>();
		healthBarRight.enabled = true;
		StartCoroutine(UpdateHealthBar());
	}

	private IEnumerator UpdateHealthBar() {
		while(true) {
			float oldFill = healthBarLeft.fillAmount;
			float newFill = health / maxHealth;
			healthBarLeft.fillAmount = Mathf.Lerp(oldFill, newFill, 0.1f);
			healthBarRight.fillAmount = Mathf.Lerp(oldFill, newFill, 0.1f);
			yield return new WaitForFixedUpdate();
		}
	}
}
