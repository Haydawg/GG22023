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
        EventsManager.Instance.TailCollidedWithHerbicides += OnTailCollidedWithHerbicides;
        EventsManager.Instance.PlayerReachedExit += OnPlayerReachedExit;
        EventsManager.Instance.WindDamage += OnWindCausesDeath;
        EventsManager.Instance.PlayerHasLostAllLives += OnPlayerHasLostAllLives;

        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            EventsManager.Instance.TailCollidedWithEnemy -= OnTailCollidedWithEnemy;
            EventsManager.Instance.TailCollidedWithHerbicides -= OnTailCollidedWithHerbicides;
            EventsManager.Instance.PlayerReachedExit -= OnPlayerReachedExit;
            EventsManager.Instance.WindDamage -= OnWindCausesDeath;
            EventsManager.Instance.PlayerHasLostAllLives -= OnPlayerHasLostAllLives;
        }
    }

    private void OnTailCollidedWithEnemy()
    {
        Debug.Log("Live lost, reason: Tail collision with enemy");
        PlayerLosesOneLive();
    }

    private void OnTailCollidedWithHerbicides()
    {
        Debug.Log("Live lost, reason: Tail collision with herbicides");
        PlayerLosesOneLive();
    }

    private void OnWindCausesDeath()
    {
        Debug.Log("Live lost, reason: Wind");
        PlayerLosesOneLive();
    }

    private void OnPlayerReachedExit()
    {
        // TODO: Load next level as soon as we have more than one
        PauseGame();
    }

    private void PlayerLosesOneLive()
    {
        remainingLives--;
        EventsManager.Instance.UpdateRemainingLivesEvent(remainingLives);

        Debug.Log("Live lost! Remaining lives: " + remainingLives);

        if (remainingLives <= 0)
        {
            EventsManager.Instance.PlayerHasLostAllLives?.Invoke();
        }
    }

    private void OnPlayerHasLostAllLives()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        Debug.Log("Time scale 0");
        Time.timeScale = 0;
    }
}
