using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    public delegate void MoveCamera(Vector3 position, bool teleport = false);
    public delegate void UpdateRemainingLives(int lives);

    public MoveCamera MoveCameraEvent;
    public UpdateRemainingLives UpdateRemainingLivesEvent;

    public UnityAction TailCollidedWithEnemy;
    public UnityAction PlayerHasLostAllLives;
    public UnityAction RestartGame;
    public UnityAction PlayerReachedExit;
    public UnityAction WindDamage;

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