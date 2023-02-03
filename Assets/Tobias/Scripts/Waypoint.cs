using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Waypoint : MonoBehaviour
{
    private string gizmoText;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (gameObject.transform.parent == null && gameObject.transform.parent.parent == null)
        {
            return;
        }

        Handles.Label(transform.position + new Vector3(0.25f, 0), "\"" + gameObject.transform.parent.parent.name + "\" " + transform.GetSiblingIndex());
    }
#endif

    public void SetupGizmoText(string text)
    {
        gizmoText = text;
    }
}
