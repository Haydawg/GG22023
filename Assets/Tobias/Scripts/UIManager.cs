using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject playerHUD;

    private void Start()
    {
        EventsManager.Instance.PlayerHasLostAllLives += ShowGameOverUI;
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            EventsManager.Instance.PlayerHasLostAllLives -= ShowGameOverUI;
        }
    }

    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }
}
