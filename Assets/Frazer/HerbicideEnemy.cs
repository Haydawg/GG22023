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

        GameObject colliderGameObjR = new GameObject("HerbicideColliderRight", typeof(EdgeCollider2D));
        rightEdgeColl = colliderGameObjR.GetComponent<EdgeCollider2D>();
        colliderGameObjR.AddComponent<HerbicideCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 forVec = gameObject.transform.up * speed;
            gameObject.transform.position = gameObject.transform.position + forVec;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 rightvec = gameObject.transform.right * speed;
            gameObject.transform.position = gameObject.transform.position + rightvec;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 downVec = gameObject.transform.up * -1 * speed;
            gameObject.transform.position = gameObject.transform.position + downVec;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 leftVec = gameObject.transform.right * -1 * speed;
            gameObject.transform.position = gameObject.transform.position + leftVec;
        }

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
