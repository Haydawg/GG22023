using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbicideEnemy : MonoBehaviour
{

    public float speed = 0.02f;
    public TrailRenderer leftTrail;
    public TrailRenderer rightTrail;
    EdgeCollider2D leftEdgeColl;
    EdgeCollider2D rightEdgeColl;
    public GameObject trailContainer;
    public EnemyController enemyCont;
    private Transform waypoint;

    // Start is called before the first frame update
    void Start()
    {
        //trail = this.GetComponent<TrailRenderer>();
        GameObject colliderGameObjL = new GameObject("HerbicideColliderLeft", typeof(EdgeCollider2D));
        leftEdgeColl = colliderGameObjL.GetComponent<EdgeCollider2D>();
        colliderGameObjL.AddComponent<HerbicideCollider>();
        colliderGameObjL.tag = "Herbicides";
        leftEdgeColl.isTrigger = true;

        GameObject colliderGameObjR = new GameObject("HerbicideColliderRight", typeof(EdgeCollider2D));
        rightEdgeColl = colliderGameObjR.GetComponent<EdgeCollider2D>();
        colliderGameObjR.AddComponent<HerbicideCollider>();
        colliderGameObjR.tag = "Herbicides";
        leftEdgeColl.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 rightvec = gameObject.transform.right * speed;
            gameObject.transform.position = gameObject.transform.position + rightvec;
            trailContainer.transform.localEulerAngles = new Vector3(0, 0, -90);
        }

        waypoint = enemyCont.GetWaypoint();
        trailContainer.transform.LookAt(waypoint);

        SetColliderTrail(leftTrail, rightTrail, leftEdgeColl, rightEdgeColl);
    }

    public void SetColliderTrail(TrailRenderer trailLeft, TrailRenderer trailRight, EdgeCollider2D leftCollider, EdgeCollider2D rightCollider)
    {
        List<Vector2> leftPoints = new List<Vector2>();
        List<Vector2> rightPoints = new List<Vector2>();
        for (int position = 0; position < trailRight.positionCount-7 || position < trailLeft.positionCount-7; position++)
        {
            Debug.Log(position);
            Vector3 rawLeft = trailLeft.GetPosition(position);
            Vector3 rawRight = trailRight.GetPosition(position);

            leftPoints.Add(rawLeft);
            rightPoints.Add(rawRight);
        }
        leftCollider.SetPoints(leftPoints);
        rightCollider.SetPoints(rightPoints);
    }
}
