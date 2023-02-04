using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootTrailTest : MonoBehaviour
{
    [SerializeField]
    private int MaxTrailSegments = 50;

    public static RootTrailTest instance;
    public float speed = 2;
    TrailRenderer trail;
    EdgeCollider2D edgeColl;

    static List<EdgeCollider2D> unusedColliders = new List<EdgeCollider2D>();

    private ArrayList playerPos;
    private ArrayList playerRot;

    private float timePassed = 0f;
    public float timeBetweenPoints = 1f;
    public List<Vector2> manualPoints = new List<Vector2>();
    public float reverseTimer = 0;

    public bool moving = false;
    public bool maxReached = false;

    public List<GameObject> spriteTrail = new List<GameObject>();
    public Sprite rootSprite;
    public Sprite oldRootSprite;
    public GameObject trailContainer;

    // Start is called before the first frame update
    void Awake()
    {
        trail = this.GetComponent<TrailRenderer>();
        GameObject colliderGameObj = new GameObject("TrailCollider", typeof(EdgeCollider2D));
        edgeColl = colliderGameObj.GetComponent<EdgeCollider2D>();
        edgeColl.isTrigger = true;
        colliderGameObj.AddComponent<Rigidbody2D>();
        colliderGameObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        colliderGameObj.AddComponent<RootTrailCollider>();
        colliderGameObj.GetComponent<RootTrailCollider>().rootTrail = this;
        //colliderGameObj.tag = "PlayerTrail";

        playerPos = new ArrayList();
        playerRot = new ArrayList();
    }

    private void Start()
    {
        EventsManager.Instance.MoveCameraEvent?.Invoke(gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (maxReached == false)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 forVec = gameObject.transform.up * speed;
                gameObject.transform.position = gameObject.transform.position + forVec;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Vector3 rightvec = gameObject.transform.right * speed;
                gameObject.transform.position = gameObject.transform.position + rightvec;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Vector3 downVec = gameObject.transform.up * -1 * speed;
                gameObject.transform.position = gameObject.transform.position + downVec;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Vector3 leftVec = gameObject.transform.right * -1 * speed;
                gameObject.transform.position = gameObject.transform.position + leftVec;
                moving = true;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ReverseGrowth();
        }
        else
        {
            timePassed = timePassed + Time.deltaTime;
            if (timePassed >= timeBetweenPoints)
            {
                if (manualPoints.Count < MaxTrailSegments)
                {
                    SetMaxReached(false);
                    if (moving == true)
                    {
                        manualPoints.Add(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
                        playerPos.Add(gameObject.transform.position);
                        playerRot.Add(gameObject.transform.localEulerAngles);

                        GameObject newPoint = new GameObject();
                        newPoint.transform.parent = trailContainer.transform;
                        newPoint.transform.position = (Vector3)playerPos[playerPos.Count -1];
                        newPoint.transform.localEulerAngles = (Vector3)playerRot[playerRot.Count - 1];
                        newPoint.transform.localScale = new Vector3(8,8,8);
                        SpriteRenderer sprite = newPoint.AddComponent<SpriteRenderer>();
                        newPoint.GetComponent<SpriteRenderer>().sprite = rootSprite;
                        sprite.sortingOrder = 5;
                        spriteTrail.Add(newPoint);

                        timePassed = 0f;
                    }
                }
                else
                {
                    SetMaxReached(true);
                }
            }
        }

        if(Input.GetKey(KeyCode.E))
        {
            MoveCenter();
        }

        SetColliderTrail(manualPoints, edgeColl); ;
    }

    private void SetColliderTrail(List<Vector2> trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        for(int position = 0; position < trail.Count; position++)
        {
            points.Add(trail[position]);
        }
        collider.SetPoints(points);
    }

    private void SetMaxReached(bool reached)
    {
        if (maxReached != reached)
        {
            if (reached == true)
            {
                Debug.Log("Maximum length reached");
            }
            maxReached = reached;
        }
    }

    public void ReverseGrowth()
    {
        reverseTimer += Time.deltaTime;
        if (reverseTimer >= 0.05f)
        {
            GrowBack();
            reverseTimer = 0;
        }
    }

    public void MoveCenter()
    {
        for(int i = 0; i < manualPoints.Count; i++)
        {
            spriteTrail[i].GetComponent<SpriteRenderer>().sprite = oldRootSprite;

        }
        
        spriteTrail.Clear();

        playerRot.Clear();
        playerPos.Clear();

        manualPoints.Clear();

        // Move camera
        EventsManager.Instance.MoveCameraEvent?.Invoke(gameObject.transform.position);
    }

    public void GrowBack(int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            if (playerPos.Count > 0 && playerRot.Count > 0)
            {
                gameObject.transform.position = (Vector3)playerPos[playerPos.Count - 1];
                playerPos.RemoveAt(playerPos.Count - 1);

                gameObject.transform.localEulerAngles = (Vector3)playerRot[playerRot.Count - 1];
                playerRot.RemoveAt(playerRot.Count - 1);

                manualPoints.RemoveAt(manualPoints.Count - 1);
                Destroy(spriteTrail[spriteTrail.Count - 1], 0);
                spriteTrail.RemoveAt(spriteTrail.Count - 1);
            }
            else
            {
                break;
            }
        }
    }

    public void GrowBackToPosition(Vector2 position)
    {
        int nearestIndex = 0;
        float shortestDistance = 10000;
        foreach(Vector2 point in manualPoints)
        {
            if((point - position).magnitude < shortestDistance)
            {
                shortestDistance = (position - point).magnitude;
                nearestIndex = manualPoints.IndexOf(point);
            }
        }

        for(int i = manualPoints.Count-1; i > 0; i--)
        {
            if(i > nearestIndex)
            {
                manualPoints.RemoveAt(i);
                spriteTrail[i].GetComponent<SpriteRenderer>().sprite = oldRootSprite;
                spriteTrail.RemoveAt(i);
                playerPos.RemoveAt(i);
                playerRot.RemoveAt(i);
            }
            else if(i == nearestIndex)
            {
                Destroy(spriteTrail[i]);
                spriteTrail.RemoveAt(i);
                playerPos.RemoveAt(i);
                playerRot.RemoveAt(i);
                manualPoints.RemoveAt(i);

                gameObject.transform.position = (Vector3)playerPos[i - 1];
                gameObject.transform.localEulerAngles = (Vector3)playerRot[i - 1];
                Destroy(spriteTrail[i-1]);
                spriteTrail.RemoveAt(i-1);
                playerPos.RemoveAt(i-1);
                playerRot.RemoveAt(i-1);
                manualPoints.RemoveAt(i-1);

                // Move camera
                EventsManager.Instance.MoveCameraEvent?.Invoke(gameObject.transform.position);
            }
        }
    }
}
