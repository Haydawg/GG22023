using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject waypointsParent;

    [SerializeField]
    private EnemyType enemyType = EnemyType.LawnMower;

    private List<Transform> waypoints;
    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex;
    private SpriteRenderer visuals;
    private Collider2D enemyCollider;
    private bool isDying;

    [SerializeField] private Animator anim;

    private void Start()
    {
        SetupWaypoints();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        currentWaypointIndex = waypoints.Count;
        SetNextWaypoint();

        visuals = GetComponentInChildren<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (isDying)
        {
            return;
        }

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            SetNextWaypoint();
        }

        visuals.flipX = navMeshAgent.velocity.x < 0;
        if(navMeshAgent.isStopped)
        {
            anim.SetBool("Is Idle", true);
        }
        else
        {
            anim.SetBool("Is Idle", false);
        }
        anim.SetFloat("Y Move", navMeshAgent.velocity.normalized.y);
        anim.SetFloat("X Move", navMeshAgent.velocity.normalized.x);
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
    }

    /**
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");

        //if (collision.gameObject.CompareTag("PlayerTrail"))
        //{
        //    Debug.Log("Player Trail collision");
        //}
    }
    **/

    public Transform GetWaypoint()
    {
        return waypoints[currentWaypointIndex];
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    public void Kill()
    {
        isDying = true;
        anim.SetBool("Is Dead", true);
        anim.SetTrigger("Die");
        //visuals.color = Color.red;
        enemyCollider.enabled = false;
        navMeshAgent.enabled = false;
        //Destroy(gameObject, 1.25f);

        if (enemyType == EnemyType.Herbicides)
        {
            var herbicideEnemy = GetComponentInChildren<HerbicideEnemy>();
            herbicideEnemy.RemoveHerbicideColliders();
        }
    }
}
