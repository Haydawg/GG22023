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

    public ArrayList playerPos;
    private ArrayList playerRot;

    public float distanceBetweenPoints = 2;
    public List<Vector2> manualPoints = new List<Vector2>();
    private float reverseTimer = 0;

    public bool moving = false;
    public bool maxReached = false;

    public List<GameObject> spriteTrail = new List<GameObject>();
    public Sprite rootSprite;
    public Sprite oldRootSprite;
    public GameObject trailContainer;

    private GameObject trailCollider;

    public List<AudioClip> growthAudio = new List<AudioClip>();
    AudioSource audioPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject colliderGameObj = new GameObject("TrailCollider", typeof(EdgeCollider2D));
        edgeColl = colliderGameObj.GetComponent<EdgeCollider2D>();
        edgeColl.isTrigger = true;
        colliderGameObj.AddComponent<Rigidbody2D>();
        colliderGameObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        colliderGameObj.AddComponent<RootTrailCollider>();
        colliderGameObj.GetComponent<RootTrailCollider>().rootTrail = this;
        //colliderGameObj.tag = "PlayerTrail";

        trailCollider = colliderGameObj;

        playerPos = new ArrayList();
        playerRot = new ArrayList();

        audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.loop = false;
        audioPlayer.playOnAwake = false;
    }

    private void Start()
    {
        CreatenewPoint();
        EventsManager.Instance.MoveCameraEvent?.Invoke(gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPointDistance = ((new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)) - manualPoints[manualPoints.Count-1]);
        if (playerPointDistance.magnitude > distanceBetweenPoints)
        {
            if (manualPoints.Count < MaxTrailSegments)
            {
                SetMaxReached(false);
                if (moving == true)
                {
                   CreatenewPoint();
                }
            }
            else
            {
                SetMaxReached(true);
            }
        }

        SetColliderTrail(manualPoints, edgeColl);

        if(moving == true)
        {
            if (audioPlayer.isPlaying == false)
            {
                int audioClipNum = Random.Range(0, growthAudio.Count - 1);
                audioPlayer.clip = growthAudio[audioClipNum];
                audioPlayer.Play();
            }
        }

        EventsManager.Instance.MoveCameraEvent?.Invoke(gameObject.transform.position);
    }

    public void CreatenewPoint()
    {
        manualPoints.Add(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
        playerPos.Add(gameObject.transform.position);
        playerRot.Add(gameObject.transform.localEulerAngles);

        GameObject newPoint = new GameObject();
        newPoint.transform.parent = trailContainer.transform;
        newPoint.transform.position = (Vector3)playerPos[playerPos.Count - 1];
        newPoint.transform.localEulerAngles = (Vector3)playerRot[playerRot.Count - 1];
        newPoint.transform.localScale = new Vector3(1, 1, 1);
        SpriteRenderer sprite = newPoint.AddComponent<SpriteRenderer>();
        newPoint.GetComponent<SpriteRenderer>().sprite = rootSprite;
        sprite.sortingOrder = 5;
        spriteTrail.Add(newPoint);
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
        if (reverseTimer >= 0.1f)
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

        CreatenewPoint();
        RootController.Instance.MoveCenter();

        // Move camera
        EventsManager.Instance.MoveCameraEvent?.Invoke(gameObject.transform.position);
    }

    public void GrowBack(int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            if (playerPos.Count > 1 && playerRot.Count > 1)
            {
                gameObject.transform.position = (Vector3)playerPos[playerPos.Count - 1];
                playerPos.RemoveAt(playerPos.Count - 1);

                gameObject.transform.localEulerAngles = (Vector3)playerRot[playerRot.Count - 1];
                playerRot.RemoveAt(playerRot.Count - 1);

                manualPoints.RemoveAt(manualPoints.Count - 1);
                Destroy(spriteTrail[spriteTrail.Count - 1], 0);
                spriteTrail.RemoveAt(spriteTrail.Count - 1);

                
                //Move Camera
                EventsManager.Instance.MoveCameraEvent?.Invoke(gameObject.transform.position);
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

        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        Invoke("TurnOnFrontCollider", 2f);
        Debug.Log("Collider Off");
    }

    public void TurnOnFrontCollider()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        Debug.Log("Collider Back On");
    }
}
