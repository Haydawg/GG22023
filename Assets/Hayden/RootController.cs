
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RootController : MonoBehaviour
{

    enum MovementMode
    {
        mouse,
        keyboard
    };

    public static RootController Instance;
    public RootTrailTest rootTrail;
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
    bool disableControlsOnReachingExit;
    [SerializeField] GameObject centrePlantPrefab;

    SpriteRenderer currentplant;
    [SerializeField] Sprite deadplant;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
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
        GameObject plant = Instantiate(centrePlantPrefab, transform.position, Quaternion.identity);
        currentplant = plant.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (disableControlsOnReachingExit)
        {
            return;
        }

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
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    rootTrail.moving = true;

                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    rootTrail.moving = false;

                }
                if (Input.GetKey(KeyCode.Mouse0) & !rootTrail.maxReached)
                {
                    float dist = Vector2.Distance(cam.ScreenToWorldPoint(Input.mousePosition), transform.position);
                    dir.Normalize();
                    Vector2 avoide = Avoidance();
                    transform.position += new Vector3(dir.x + avoide.x, dir.y + avoide.y, 0).normalized * speed * Time.deltaTime;

                }

                if (Input.GetKey(KeyCode.Mouse1))
                    rootTrail.ReverseGrowth();

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
        
            hits = Physics2D.RaycastAll(transform.position, transform.TransformVector(rayVector), rayDist);

            if (hits.Length > 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.tag == "Obstacle")
                    {
                        avoidAmount += Vector2.Reflect(transform.TransformVector(ray.direction), hit.normal) * avoidenceWeight;
                        hitCount++;
                    }
                }
            }
        }
        
        if (hitCount > 0)
            avoidAmount = avoidAmount / Mathf.Max(hitCount, 1);
        return avoidAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log(collision.gameObject.name);
            rootTrail.MoveCenter();
            //Destroy(collision.gameObject);

            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            EnemyType enemyType = enemyController.GetEnemyType();

            if (enemyType == EnemyType.WeedWacker || enemyType == EnemyType.Herbicides)
            {
                enemyController.Kill();

                Debug.Log("Head collided with killable enemy");
            }
            else if (enemyType == EnemyType.LawnMower)
            {
                Vector3 hitLocation = collision.gameObject.transform.position;
                rootTrail.GrowBackToPosition(hitLocation);
                EventsManager.Instance.HeadCollidedWithInvincibleEnemy?.Invoke();

                Debug.Log("Head collided with LawnMower");
            }
        }
        else if (collision.CompareTag("Herbicides"))
        {
            Debug.Log("Root collided with Herbicides");
        }
        else if (collision.CompareTag("Exit"))
        {
            Debug.Log("Player reached exit");
            EventsManager.Instance.PlayerReachedExit?.Invoke();
            disableControlsOnReachingExit = true;
        }
    }

    public void TrailCollidedWithEnemy(GameObject trailColliderObject, Collider2D collision)
    {
        Debug.Log("Trail collision");
        //rootTrail.GrowBack(20);
        rootTrail.GrowBackToPosition(collision.transform.position);
    }

    public void MoveCenter()
    {
        currentplant.sprite = deadplant;
        GameObject newPlant = Instantiate(centrePlantPrefab, transform.position, Quaternion.identity);
        currentplant = newPlant.GetComponentInChildren<SpriteRenderer>();
    }
}