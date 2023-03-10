using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
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
            
        Handles.Label(transform.position + new Vector3(0.5f, 0.1f), "<color=green>" + gameObject.transform.parent.name + "</color>", new GUIStyle { richText = true, fontStyle = FontStyle.Bold });
    }
#endif
}