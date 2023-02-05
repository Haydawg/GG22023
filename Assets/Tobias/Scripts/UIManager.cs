using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject gameWonUI;

    [SerializeField]
    private PlayerHUD playerHUD;

    private void Start()
    {
        EventsManager.Instance.PlayerHasLostAllLives += ShowGameOverUI;
        EventsManager.Instance.PlayerReachedExit += OnPlayerReachedExit;
        EventsManager.Instance.UpdateRemainingLivesEvent += OnUpdateRemainingLives;
    }

    private void OnUpdateRemainingLives(int lives)
    {
        playerHUD.UpdateLives(lives);
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            EventsManager.Instance.PlayerHasLostAllLives -= ShowGameOverUI;
            EventsManager.Instance.PlayerReachedExit -= OnPlayerReachedExit;
            EventsManager.Instance.UpdateRemainingLivesEvent -= OnUpdateRemainingLives;
        }
    }

    private void ShowGameOverUI()
    {
        playerHUD.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }

    private void OnPlayerReachedExit()
    {
        playerHUD.gameObject.SetActive(false);
        gameWonUI.SetActive(true);
    }

    public void SetupPlayerLives(int lives)
    {
        playerHUD.UpdateLives(lives);
    }
}
