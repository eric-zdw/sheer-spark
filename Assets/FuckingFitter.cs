using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckingFitter : MonoBehaviour
{
    private float height;
    private UnityEngine.UI.LayoutElement layoutElement;

    // Start is called before the first frame update
    void Start()
    {
        layoutElement = GetComponent<UnityEngine.UI.LayoutElement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        layoutElement.minWidth = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
    }
}
