using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDisplay : MonoBehaviour {

    public Color[] colors;
    public List<MeshRenderer> renderers;
    private MaterialPropertyBlock materialPropertyBlock;
    private MaterialPropertyBlock pmpb;
    public ParticleSystemRenderer particle;

	// Use this for initialization
	void Start () {
        //Destroy(gameObject, 8f);
        materialPropertyBlock = new MaterialPropertyBlock();
        pmpb = new MaterialPropertyBlock();
        StartCoroutine(ChangeColors());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator ChangeColors() {
        while (true) {  
            Color c1 = colors[5];
            Color c2;
            foreach (Color c in colors) {
                c2 = c;
                float t = 0f;
                while (t < 1f) {
                    SetColor(c1, c2, t);
                    t += 0.002f;
                    yield return new WaitForSecondsRealtime(0.02f);
                }
                c1 = c2;
            }
        }
    }

    private void SetColor(Color c1, Color c2, float t) {
        float r = Mathf.Lerp(c1.r, c2.r, t);
        float g = Mathf.Lerp(c1.g, c2.g, t);
        float b = Mathf.Lerp(c1.b, c2.b, t);
        Color newColor = new Color(r, g, b);
        materialPropertyBlock.SetColor("_TintColor", newColor);
        foreach (MeshRenderer ren in renderers) {
            ren.SetPropertyBlock(materialPropertyBlock);
        }

        pmpb.SetColor("_TintColor", newColor);
        particle.SetPropertyBlock(pmpb);
    }
}
