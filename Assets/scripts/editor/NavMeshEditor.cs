using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(NavMeshGenerator))]
public class NavMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NavMeshGenerator navScript = (NavMeshGenerator)target;
        if(GUILayout.Button("Build Mesh"))
        {
            navScript.GenerateMesh();
        }
    }
}