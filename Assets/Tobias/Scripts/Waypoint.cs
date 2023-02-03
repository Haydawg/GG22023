using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private string gizmoText;

    void OnDrawGizmos()
    {
        Handles.Label(transform.position, gizmoText);
    }

    public void SetupGizmoText(string text)
    {
        gizmoText = text;
    }
}
