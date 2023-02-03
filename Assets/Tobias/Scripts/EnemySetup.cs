using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemySetup : MonoBehaviour
{
    [SerializeField]
    private Transform waypointsParent;

    public void UpdateWaypoints()
    {
        Debug.Log("EnemySetup: Update waypoints");

        Waypoint[] waypoints = waypointsParent.GetComponentsInChildren<Waypoint>();

        if (waypoints.Length == 0)
        {
            Debug.LogError("EnemySetup: No waypoints found!");
            return;
        }

        int i = 0;
        foreach (Waypoint waypoint in waypoints)
        {
            waypoint.SetupGizmoText("\"" + gameObject.name + "\" " + i);
            i++;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemySetup))]
[CanEditMultipleObjects]
public class EnemySetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Update Waypoints"))
        {
            EnemySetup enemySetup = target as EnemySetup;
            enemySetup.UpdateWaypoints();
        }
    }
}
#endif