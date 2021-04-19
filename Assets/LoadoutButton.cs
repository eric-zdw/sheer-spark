using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutButton : MonoBehaviour
{

    private UnityEngine.UI.Button button;
    public WeaponColor weaponColor;
    public int weaponSelection;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(SwitchWeapon);
    }

    private void Update()
    {
        if (SaveManager.GetSelectedWeapon(weaponColor) == weaponSelection)
        {
            button.image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            button.image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }

    void SwitchWeapon()
    {
        SaveManager.SetSelectedWeapon(weaponColor, weaponSelection);
        GameObject pm = GameObject.Find("PowerupManager");
        if (pm != null) {
            pm.GetComponent<PowerupManager>().UpdatePowerups();
        }
    }
}
