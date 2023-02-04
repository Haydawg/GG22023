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

    private void Awake()
    {
        EventsManager.Instance.TailCollidedWithEnemy += OnTailCollidedWithEnemy;
        EventsManager.Instance.RestartGame += OnRestartGame;
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            EventsManager.Instance.TailCollidedWithEnemy -= OnTailCollidedWithEnemy;
            EventsManager.Instance.RestartGame -= OnRestartGame;
        }
    }

    private void OnTailCollidedWithEnemy()
    {
        remainingLives--;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Debug.isDebugBuild)
        {
            OnRestartGame();
        }
    }
}
