using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuFlowManager : MonoBehaviour
{
    [SerializeField]
    private string gameScene;

    private bool loadingGameScene;

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(gameScene);
        loadingGameScene = true;
    }

    public void ExitGame()
    {
        if (loadingGameScene)
        {
            return;
        }

        if (Application.isEditor)
        {
#if UNTIY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        }
        else
        {
            Application.Quit();
        }
#endif
    }
}
