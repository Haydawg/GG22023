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
    private GameObject playerHUD;

    private void Start()
    {
        EventsManager.Instance.PlayerHasLostAllLives += ShowGameOverUI;
        EventsManager.Instance.PlayerReachedExit += OnPlayerReachedExit;
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            EventsManager.Instance.PlayerHasLostAllLives -= ShowGameOverUI;
            EventsManager.Instance.PlayerReachedExit -= OnPlayerReachedExit;
        }
    }

    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    private void OnPlayerReachedExit()
    {
        gameWonUI.SetActive(true);
    }
}
