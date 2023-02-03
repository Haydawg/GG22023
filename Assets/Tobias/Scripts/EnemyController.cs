using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject waypointsParent;

    private List<Transform> waypoints;
    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex;

    private void Start()
    {
        SetupWaypoints();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        currentWaypointIndex = waypoints.Count;
        SetNextWaypoint();
    }

    private void Update()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Count)
        {
            currentWaypointIndex = 0;
        }

        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private void SetupWaypoints()
    {
        Transform[] childTransforms = waypointsParent.transform.GetComponentsInChildren<Transform>();

        if (childTransforms.Length == 0)
        {
            Debug.LogError("EnemyController: No Waypoints found!");
            return;
        }

        waypoints = new List<Transform>(childTransforms);
        waypoints.Remove(waypointsParent.transform);

        foreach (Transform waypointTransform in waypoints)
        {
            Waypoint waypoint = waypointTransform.gameObject.GetComponent<Waypoint>();
            waypoint.SetupGizmoText(gameObject.name);
        }
    }
}
