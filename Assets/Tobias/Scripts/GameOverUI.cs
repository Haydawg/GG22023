using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button quitButton;

    private void Start()
    {
        restartButton.onClick.AddListener(EventsManager.Instance.RestartGame);
        quitButton.onClick.AddListener(EventsManager.Instance.QuitGame);
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
