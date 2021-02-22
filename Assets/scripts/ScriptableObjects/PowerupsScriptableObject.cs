using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerupList", menuName = "ScriptableObjects/PowerupList", order = 1)]
public class PowerupsScriptableObject : ScriptableObject
{
    public GameObject[] redWeapons;
    public GameObject[] orangeWeapons;
    public GameObject[] yellowWeapons;
    public GameObject[] greenWeapons;
    public GameObject[] blueWeapons;
    public GameObject[] purpleWeapons;
}
