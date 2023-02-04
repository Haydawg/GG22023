using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindController : MonoBehaviour
{
    public static WindController Instance;
    RootTrailTest rootTrail;
    [SerializeField] List<ParticleSystem> particles = new List<ParticleSystem>();
    [SerializeField] GameObject wind;

    [SerializeField] float windLevel;
    float windChangeTimer;
    [SerializeField] float windChangeInterval;
    bool warning;
    [SerializeField] Image windLevelMeter;
    [SerializeField] GameObject warningAlarm;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        warningAlarm.SetActive(false);
        wind.SetActive(false);
        rootTrail = RootController.Instance.rootTrail;
    }

    // Update is called once per frame
    void Update()
    {
        windChangeTimer += Time.deltaTime;

        if (windChangeTimer > windChangeInterval)
        {
            float amount = Random.Range(-1f, 1f);
            if (windLevel + amount > 0 & windLevel + amount < 20)
                windLevel += amount;
            windChangeTimer = 0;
        }

        if(windLevel > 15)
        {
            if(!warning)
                Warning();
        }
        else
        {
            if (warning)
            {
                warningAlarm.SetActive(false);
                warning = false;
                wind.SetActive(false);
            }
        }
        windLevelMeter.fillAmount = windLevel / 20;
    }

    void Warning()
    {
        warning = true;
        warningAlarm.SetActive(true);
        wind.SetActive(true);

    }
}
