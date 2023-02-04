using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameSessionManager : MonoBehaviour
{
    [SerializeField]
    private int remainingLives = 3;

    [SerializeField]
    private UIManager uiManager;

    private void Start()
    {
        uiManager.SetupPlayerLives(remainingLives);
    }

    private void Awake()
    {
        EventsManager.Instance.TailCollidedWithEnemy += OnTailCollidedWithEnemy;
        EventsManager.Instance.RestartGame += OnRestartGame;
        EventsManager.Instance.PlayerReachedExit += OnPlayerReachedExit;
        EventsManager.Instance.WindDamage += OnWindCausesDeath;
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            EventsManager.Instance.TailCollidedWithEnemy -= OnTailCollidedWithEnemy;
            EventsManager.Instance.RestartGame -= OnRestartGame;
            EventsManager.Instance.PlayerReachedExit -= OnPlayerReachedExit;
            EventsManager.Instance.WindDamage -= OnWindCausesDeath;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Debug.isDebugBuild)
        {
            OnRestartGame();
        }
    }

    private void OnTailCollidedWithEnemy()
    {
        remainingLives--;
        EventsManager.Instance.UpdateRemainingLivesEvent(remainingLives);

        Debug.Log("Live lost! Remaining lives: " + remainingLives);

        if (remainingLives <= 0)
        {
            EventsManager.Instance.PlayerHasLostAllLives?.Invoke();
        }
    }
    private void OnWindCausesDeath()
    {
        remainingLives--;
        EventsManager.Instance.UpdateRemainingLivesEvent(remainingLives);
        Debug.Log("Live lost! Remaining lives: " + remainingLives);
        if (remainingLives <= 0)
        {
            EventsManager.Instance.PlayerHasLostAllLives?.Invoke();
        }
    }

    private void OnRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnPlayerReachedExit()
    {
        // TODO: Load next level as soon as we have more than one
    }
}
