using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GardenerController : MonoBehaviour
{
    [SerializeField]
    private Transform destination;

    private NavMeshAgent meshAgent;

    // Start is called before the first frame update
    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        meshAgent.updateRotation = false;
        meshAgent.updateUpAxis = false;

        //meshAgent.SetDestination(destination.position);
    }

    void Update()
    {
        meshAgent.SetDestination(destination.position);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse down");
            meshAgent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
