using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

public class EnemyCustomGizmo : MonoBehaviour
{

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (gameObject.transform.parent == null)
        {
            return;
        }
            
        Handles.Label(transform.position + new Vector3(0.25f, 0), gameObject.transform.parent.name);
    }
#endif
}