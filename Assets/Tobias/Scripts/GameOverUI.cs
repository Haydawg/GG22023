using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;

    private void Start()
    {
        restartButton.onClick.AddListener(EventsManager.Instance.RestartGame);
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance)
        {
            restartButton.onClick.RemoveListener(EventsManager.Instance.RestartGame);
        }
    }
}
