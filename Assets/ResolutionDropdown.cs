using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour
{
    Resolution[] resolutions;
    Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        dropdown = GetComponent<UnityEngine.UI.Dropdown>();

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (Resolution r in resolutions)
        {
            options.Add(new Dropdown.OptionData(r.ToString()));
        }

        dropdown.options = options;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeResolution()
    {
        string resolutionString = GetComponent<UnityEngine.UI.Dropdown>().itemText.text;
        string[] split = resolutionString.Split('x', ' ');
        print(split[0] + " " + split[1]);
        Resolution newResolution = new Resolution();
        //newResolution.width = ()
        //Screen.SetResolution()
    }
}
