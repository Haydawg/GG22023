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

    private GameObject leftHerbicideCollider;
    private GameObject rightHerbicideCollider;

    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        //trail = this.GetComponent<TrailRenderer>();
        leftHerbicideCollider = new GameObject("HerbicideColliderLeft", typeof(EdgeCollider2D));
        leftEdgeColl = leftHerbicideCollider.GetComponent<EdgeCollider2D>();
        leftHerbicideCollider.AddComponent<HerbicideCollider>();
        leftHerbicideCollider.tag = "Herbicides";
        leftEdgeColl.isTrigger = true;

        rightHerbicideCollider = new GameObject("HerbicideColliderRight", typeof(EdgeCollider2D));
        rightEdgeColl = rightHerbicideCollider.GetComponent<EdgeCollider2D>();
        rightHerbicideCollider.AddComponent<HerbicideCollider>();
        rightHerbicideCollider.tag = "Herbicides";
        rightEdgeColl.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Debug.isDebugBuild && Input.GetKey(KeyCode.D))
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
        for (int position = 0; position < trailRight.positionCount-8 || position < trailLeft.positionCount-8; position++)
        {
            //Debug.Log(position);
            Vector3 rawLeft = trailLeft.GetPosition(position);
            Vector3 rawRight = trailRight.GetPosition(position);

            leftPoints.Add(rawLeft);
            rightPoints.Add(rawRight);
        }
        leftCollider.SetPoints(leftPoints);
        rightCollider.SetPoints(rightPoints);
    }

    public void RemoveHerbicideColliders()
    {
        Destroy(leftHerbicideCollider);
        Destroy(rightHerbicideCollider);

        //leftTrail.startColor = new Color(leftTrail.startColor.r, leftTrail.startColor.g, leftTrail.startColor.b, 64);
        //leftTrail.endColor = new Color(leftTrail.endColor.r, leftTrail.endColor.g, leftTrail.endColor.b, 64);

        //rightTrail.startColor = new Color(rightTrail.startColor.r, rightTrail.startColor.g, rightTrail.startColor.b, 64);
        //rightTrail.endColor = new Color(rightTrail.endColor.r, rightTrail.endColor.g, rightTrail.endColor.b, 64);

        leftTrail.startColor = new Color(255, 255, 255, 0);
        leftTrail.endColor = new Color(255, 255, 255, 0);

        rightTrail.startColor = new Color(255, 255, 255, 0);
        rightTrail.endColor = new Color(255, 255, 255, 0);

        isDead = true;
    }
}
