using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterFlash : MonoBehaviour
{
    private UnityEngine.UI.Image img;
    private float alpha = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<UnityEngine.UI.Image>();
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        Color c = new Color(img.color.r, img.color.g, img.color.b, alpha);
        img.color = c;
        alpha -= Time.deltaTime * 0.3f;
    }
}
