using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeMap", menuName = "ScriptableObjects/NodeMap", order = 1)]
public class NodeMapScriptableObject : ScriptableObject
{
    public List<List<GameObject>> nodes;
}
