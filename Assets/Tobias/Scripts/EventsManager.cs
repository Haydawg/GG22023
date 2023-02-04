using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    public delegate void MoveCamera(Vector3 position, bool teleport = false);

    public MoveCamera MoveCameraEvent;

    public UnityAction TailCollidedWithEnemy;
    public UnityAction PlayerHasLostAllLives;
    public UnityAction RestartGame;
    public UnityAction PlayerReachedExit;

    public static EventsManager Instance;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            Debug.LogError("More than 1 EventsManager exists!");
        }
        else
        {
            Instance = this;
        }
    }
}