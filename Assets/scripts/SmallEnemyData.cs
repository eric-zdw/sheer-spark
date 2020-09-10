using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SmallEnemyScriptableObject", menuName = "ScriptableObjects/SmallEnemyScriptableObject", order = 1)]
public class SmallEnemyScriptableObject : ScriptableObject
{
    public GameObject[] deathExplosions;
    public Color[] powerupColors;

    [ColorUsage(true, true)]
    public Color[] powerupColorsHDR;

    public UnityEditor.Presets.Preset[] outlines;

    public GameObject healthBarPrefab;
}
