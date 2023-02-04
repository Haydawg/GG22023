using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameSceneManager : MonoBehaviour
{
    [SerializeField]
    private string mainMenuScene;

    private void Awake()
    {
        EventsManager.Instance.RestartGame += OnRestartGame;
        EventsManager.Instance.QuitGame += OnQuitGame;
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            EventsManager.Instance.RestartGame -= OnRestartGame;
            EventsManager.Instance.QuitGame -= OnQuitGame;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Debug.isDebugBuild)
        {
            OnRestartGame();
        }
    }

    private void OnQuitGame()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    private void OnRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
