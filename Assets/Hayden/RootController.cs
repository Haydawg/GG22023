using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RootController : MonoBehaviour
{

    enum MovementMode
    {
        mouse,
        keyboard
    };

    public static RootController Instance;

    [SerializeField] MovementMode movementType;
    [SerializeField] float speed;
    private Vector2 move;
    [SerializeField] float distBetweenPathNodes;

    private Camera cam;
    private CharacterController controller;
    List<Transform> pathnodes = new List<Transform>();
    [SerializeField] float rayAngle;
    [SerializeField] float rayDist;
    [SerializeField] float avoidenceWeight;

    List<Vector3> rayVectors;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
        pathnodes.Add(transform);
        rayVectors = new List<Vector3>();
        rayVectors.Add(Quaternion.AngleAxis(rayAngle, Vector3.up) * (Vector3.right + Vector3.up) * rayDist);
        rayVectors.Add(Quaternion.AngleAxis(-rayAngle, Vector3.up) * (-Vector3.right + Vector3.up) * rayDist);
        rayVectors.Add(Vector3.up * rayDist);
    }

    // Update is called once per frame
    void Update()
    {
        switch (movementType)
        {
            case MovementMode.keyboard:
                move = Vector2.zero;
                move += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                controller.Move(move * Time.deltaTime);
                break;
            case MovementMode.mouse:
                Vector2 dir = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    float dist = Vector2.Distance(cam.ScreenToWorldPoint(Input.mousePosition), transform.position);
                    dir.Normalize();
                    Vector2 avoide = Avoidance();
                    transform.position += new Vector3(dir.x + avoide.x , dir.y + avoide.y , 0).normalized  * speed * Time.deltaTime;
                    
                }
                if(Input.GetKeyDown(KeyCode.Mouse1))
                {

                }
                break;
        }
       

    }
    public List<Transform> GetPathNodes()
    {
        return pathnodes;
    }

    Vector2 Avoidance()
    {
        Vector2 avoidAmount = Vector2.zero;
        int hitCount = 0;
        RaycastHit2D[] hits;
        foreach (Vector3 rayVector in rayVectors)
        {
            Ray2D ray = new Ray2D(transform.position, rayVector);
            Debug.DrawRay(transform.position, transform.TransformVector(rayVector), Color.red);
            
            hits = Physics2D.RaycastAll(transform.position, rayVector, rayDist);
            Debug.Log(ray.direction);

            if (hits.Length > 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.tag == "Obstacle")
                    {
                        avoidAmount += Vector2.Reflect(ray.direction, hit.normal).normalized * avoidenceWeight;
                        hitCount++;
                    }
                }
            }
        }
        if (hitCount > 0)
            avoidAmount = avoidAmount / Mathf.Max(hitCount, 1);
        return avoidAmount.normalized;
    }
}
