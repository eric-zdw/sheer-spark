using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

	public UnityEngine.UI.Image img;

	// Use this for initialization
	void Start () {
		img = GetComponent<UnityEngine.UI.Image>();	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	IEnumerator FadeIn() {
		while (img.color.a > 0f)
		{
			img.color = new Color(img.color.r, img.color.b, img.color.g, img.color.a - 0.8f * Time.deltaTime);
			yield return null;
		}
		Destroy(gameObject);
	}

	IEnumerator FadeOut() {
		while (img.color.a < 1f)
		{
			img.color = new Color(img.color.r, img.color.b, img.color.g, img.color.a + 0.8f * Time.deltaTime);
			yield return null;
		}
	}


}
