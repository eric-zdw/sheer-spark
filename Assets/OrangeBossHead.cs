using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBossHead : Enemy {

	public Transform[] travelLines;
	private List<Transform> path;
	public float speed = 6f;
	private float fluctuation = 0f;
	public float damageMultiplier = 1.1f;

	public int numSegments = 5;
	public GameObject bodySegment;

	private List<GameObject> segments;
	private List<Vector3> positionHistory;

	private UnityEngine.UI.Image healthBarLeft;
	private UnityEngine.UI.Image healthBarRight;

	private MaterialPropertyBlock damageMatBlock;
	private MeshRenderer damageFlash;

	public float newHealth = 1000f;

	// Use this for initialization
	void Start () {
		maxHealth = newHealth;
		health = maxHealth;
		segments = new List<GameObject>();
		for (int i = 0; i < numSegments; i++) {
			segments.Add(Instantiate(bodySegment, transform.position, Quaternion.identity, transform.parent));
			segments[i].GetComponent<OrangeBossPiece>().head = this;
		}

		damageFlash = transform.GetChild(2).GetComponent<MeshRenderer>();
		damageMatBlock = new MaterialPropertyBlock();

		positionHistory = new List<Vector3>();
		for (int i = 0; i < 1000; i++) {
			positionHistory.Add(Vector3.zero);
		}
		StartCoroutine(BossRoutine2());
		StartCoroutine(Fluctuate());

		ActivateHealthBar();
	}

	public IEnumerator BossRoutine1() {
		bool switchToNewRoutine = false;
		while (!switchToNewRoutine) {
			//select a travelline
			int selection = Random.Range(0, travelLines.Length);
			path = new List<Transform>();

			foreach (Transform t in travelLines[selection]) {
				path.Add(t);
			}

			//Teleport to first waypoint and travel down travelline
			transform.position = path[0].position;
			foreach (Transform t in path) {
				Vector3 direction = t.position - transform.position;
				
				while (Vector3.Distance(transform.position, t.position) > 1f) {
					transform.position += Vector3.Normalize(direction) * (speed + fluctuation) * Time.deltaTime;
					UpdatePositions(8);
					yield return new WaitForFixedUpdate();
				}
			}

			//chance to transition to second routine
			if (Random.Range(0f, 1f) < 0.2f) {
				switchToNewRoutine = true;
			}
		}

		StartCoroutine(BossRoutine2());
	}

	public IEnumerator BossRoutine2() {
		//select a travelline
		int selection = Random.Range(0, travelLines.Length);
		float slowSpeed = speed * 5f;
		
		path = new List<Transform>();
		foreach (Transform t in travelLines[selection]) {
			path.Add(t);
		}

		//Teleport to first waypoint and travel down travelline
		//Do not interpolate with rigidbody MovePosition
		transform.position = path[0].position;

		Vector3 direction = path[1].position - transform.position;
		
		//slow down into centre
		while (slowSpeed > 3f) {
			transform.position += Vector3.Normalize(direction) * slowSpeed * Time.deltaTime;
			UpdatePositions(8);
			slowSpeed *= 0.98f;
			yield return new WaitForFixedUpdate();
		}
		float wait = 0f;
		while (wait < 200f) {
			transform.position += Vector3.Normalize(direction) * slowSpeed * Time.deltaTime;
			UpdatePositions(8);
			//shoot stuf

			if (wait % 2 == 0) {
				
			}

			wait++;
			yield return new WaitForFixedUpdate();
		}
		while (slowSpeed < speed * 8f) {
			transform.position += Vector3.Normalize(direction) * slowSpeed * Time.deltaTime;
			UpdatePositions(8);
			slowSpeed *= 1.02f;
			yield return new WaitForFixedUpdate();
		}

		StartCoroutine(BossRoutine1());
	}

	public IEnumerator Fluctuate() {
		float rad = 0f;
		while (true) {
			rad += Time.deltaTime * 2f;
			fluctuation = Mathf.Sin(rad) * 5f;
			yield return new WaitForFixedUpdate();
		}
	}

	public void UpdatePositions(int spacing) {
		positionHistory.Insert(0, transform.position);
		for (int i = 0; i < segments.Count; i++) {
			//offset each segment by (spacing) physics steps
			segments[i].GetComponent<Rigidbody>().MovePosition(positionHistory[(i + 1) * spacing]);
			//segments[i].transform.position = positionHistory[(i + 1) * spacing];
		}

		//remove last location
		positionHistory.RemoveAt(positionHistory.Count - 1);
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

	public override void getDamage(float damage)
    {
        health -= damage;
		StartCoroutine(FlashWhite());
    }

	public void getDamageNoFlash(float damage)
    {
        health -= damage;
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

	private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(1);
        }
    }
}
