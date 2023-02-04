using UnityEngine;
using UnityEngine.UI;

public class GameWonUI : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button quitButton;

    private void Start()
    {
        restartButton.onClick.AddListener(EventsManager.Instance.RestartGame);
        restartButton.onClick.AddListener(() => Debug.Log("RESTART GAME"));
        quitButton.onClick.AddListener(EventsManager.Instance.QuitGame);
        quitButton.onClick.AddListener(() => Debug.Log("QUIT GAME"));
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            restartButton.onClick.RemoveListener(EventsManager.Instance.RestartGame);
            quitButton.onClick.RemoveListener(EventsManager.Instance.QuitGame);
        }
    }
}
