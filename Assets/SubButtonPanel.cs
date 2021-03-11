using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubButtonPanel : MonoBehaviour
{
    private UnityEngine.UI.Image image;

    private float fillVelocity = 0f;

    private bool hiding = false;
    private bool revealing = false;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        image.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (revealing && image.fillAmount < 1f)
        {
            image.fillAmount = Mathf.SmoothDamp(image.fillAmount, 1f, ref fillVelocity, 0.15f);
        }
        else if (hiding && image.fillAmount > 0f)
        {
            image.fillAmount = Mathf.SmoothDamp(image.fillAmount, 0f, ref fillVelocity, 0.15f);
        }
    }

    public void Reveal()
    {
        if (hiding)
        {
            hiding = false;
        }

        revealing = true;
    }
    public void Hide()
    {
        if (revealing)
        {
            revealing = false;
        }

        hiding = true;
    }
}
