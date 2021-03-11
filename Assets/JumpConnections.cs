using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JumpConnections : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    [DrawGizmo(GizmoType.InSelectionHierarchy |
                GizmoType.NotInSelectionHierarchy |
                GizmoType.Pickable)]
    */
    void OnDrawGizmos() {
        Color color1 = new Color(0f, 0f, 1f);
        Color color2 = new Color(1f, 0.5f, 0f);
        
        foreach (Transform t in transform) {
            if (t.childCount == 2)
            {
                Gizmos.color = color1;
                Gizmos.DrawSphere(t.GetChild(0).position, 0.4f);
                Gizmos.color = color2;
                Gizmos.DrawLine(t.GetChild(0).position, t.GetChild(1).position);
                Gizmos.DrawSphere(t.GetChild(1).position, 0.4f);
            }
        }
    }
}
