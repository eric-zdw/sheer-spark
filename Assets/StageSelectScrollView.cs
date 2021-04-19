using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectScrollView : MonoBehaviour
{
    public RectTransform rect;

    // Expressed as a fraction of screen height.
    public float heightPerItem = 0.05f;
    private int screenHeight;
    private int numberOfItemsInList;

    // Start is called before the first frame update
    void Start()
    {
        screenHeight = Screen.height;
        numberOfItemsInList = transform.childCount;
        Resize();
    }

    // Update is called once per frame
    void Update()
    {
        if (screenHeight != Screen.height)
        {
            screenHeight = Screen.height;
            Resize();
        }
        if (numberOfItemsInList != transform.childCount)
        {
            Resize();
        }
    }

    void Resize()
    {
        numberOfItemsInList = transform.childCount;
        int pixelHeightPerItem = (int)(Screen.height * heightPerItem);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, pixelHeightPerItem * numberOfItemsInList);
    }
}
