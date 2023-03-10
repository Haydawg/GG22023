using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Text lengthUsed;


    [SerializeField] GameObject livesPanel;
    [SerializeField] GameObject heartPrefab;

    private void Awake()
    {
        EventsManager.Instance.UpdateRemainingLivesEvent += UpdateLives;
    }

    // Start is called before the first frame update
    void Start()
    {
        //UpdateLives(3);

        // Called from GameSessionManager/UIManager
    }

    // Update is called once per frame
    void Update()
    {
        lengthUsed.text = RootController.Instance.rootTrail.manualPoints.Count.ToString();
    }

    public void UpdateLives(int lives)
    {
        foreach (Transform child in livesPanel.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < lives; i++)
        {
            Instantiate(heartPrefab, livesPanel.transform);
        }
    }
}
