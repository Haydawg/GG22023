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

    // Start is called before the first frame update
    void Start()
    {
        //trail = this.GetComponent<TrailRenderer>();
        GameObject colliderGameObjL = new GameObject("HerbicideColliderLeft", typeof(EdgeCollider2D));
        leftEdgeColl = colliderGameObjL.GetComponent<EdgeCollider2D>();
        colliderGameObjL.AddComponent<HerbicideCollider>();
        colliderGameObjL.tag = "Enemy";

        GameObject colliderGameObjR = new GameObject("HerbicideColliderRight", typeof(EdgeCollider2D));
        rightEdgeColl = colliderGameObjR.GetComponent<EdgeCollider2D>();
        colliderGameObjR.AddComponent<HerbicideCollider>();
        colliderGameObjR.tag = "Enemy";
    }

    // Update is called once per frame
    void Update()
    {
       
        SetColliderTrail(leftTrail, rightTrail, leftEdgeColl, rightEdgeColl);
    }

    public void SetColliderTrail(TrailRenderer trailLeft, TrailRenderer trailRight, EdgeCollider2D leftCollider, EdgeCollider2D rightCollider)
    {
        List<Vector2> leftPoints = new List<Vector2>();
        List<Vector2> rightPoints = new List<Vector2>();
        for (int position = 0; position < trailRight.positionCount-1 || position < trailLeft.positionCount-1; position++)
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
