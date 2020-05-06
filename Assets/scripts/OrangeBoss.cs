using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBoss : Enemy {
	public float newHealth;

	private GameObject player;
	private Vector3 playerLocation;
	private Rigidbody rb;

    public float YLimit;
    private int powerupRoll;

	private MaterialPropertyBlock damageMatBlock;

	private int numberOfAttacks = 6;
	private int numberOfAttacksPhaseTwo = 2;

	private enum BossAttack {RedSpread, OrangeMines, YellowMissiles, GreenVortex, BlueTornado, PurpleBeams};
	private enum BossAttackPhaseTwo {WhiteLaserColumns, WhiteLaserSpinning};

	private UnityEngine.UI.Image healthBarLeft;
	private UnityEngine.UI.Image healthBarRight;

	private List<GameObject> auras;

	public GameObject redSpreadProjectile;

    void Start()
	{
		maxHealth = newHealth;
		health = maxHealth;

		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();

		powerupRoll = Random.Range(0, 6);

		auras = new List<GameObject>();
		foreach (Transform child in transform) {
			auras.Add(child.gameObject);
		}

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
            //Explode();
        }
    }

    void Explode()
	{
		Camera.main.GetComponent<CameraFollow>().AddNoise(10f);
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
			yield return new WaitForSeconds(10f);

			//Attack period
			//BossAttack chooseAttack = (BossAttack)Random.Range(1, numberOfAttacks+1);
			BossAttack chooseAttack = BossAttack.RedSpread;

			if (chooseAttack == BossAttack.RedSpread) {
				StartCoroutine(BossAttack_RedSpread());
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

	private IEnumerator BossAttack_RedSpread() {
		//phase one: rotate to speed

		yield return new WaitForSeconds(0.1f);
		float timer = 0f;
		while (timer < 6f) {
			timer += 1.2f;

			for (int i = 0; i <= 40f; i++) {
				float randomSpread = Random.Range(0f, 360f);
				Instantiate(redSpreadProjectile, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, randomSpread));
			}

			yield return new WaitForSeconds(1.2f);
		}
	}
}
