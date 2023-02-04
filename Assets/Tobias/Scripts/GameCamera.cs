using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 100.0f;

    private Vector3 targetPosition;

    private void Awake()
    {
        EventsManager.Instance.MoveCameraEvent += SetTargetPosition;
        targetPosition = gameObject.transform.position;
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            EventsManager.Instance.MoveCameraEvent -= SetTargetPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void SetTargetPosition(Vector3 targetPosition, bool teleport)
    {
        if (teleport)
        {

        }
        else
        {
            this.targetPosition = new Vector3(targetPosition.x, targetPosition.y, this.targetPosition.z);
        }
    }
}
