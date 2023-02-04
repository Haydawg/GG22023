using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    public UnityAction TailCollidedWithEnemy;
    public UnityAction PlayerHasLostAllLives;
    public UnityAction RestartGame;

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