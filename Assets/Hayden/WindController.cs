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

    [SerializeField] float windRampUpDelay;

    [SerializeField] bool windBuilding;
    float targetWind;
    bool causedDamage = false;
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
        if (windChangeTimer > windRampUpDelay + Random.Range(0, 60))
        {
            windBuilding = true;
            windChangeTimer = 0;
        }
        if (!windBuilding)
        {
            targetWind = 5 + (Mathf.Sin(Time.timeSinceLevelLoad * 0.5f));
            if (targetWind > 0 & targetWind < 10)
                windLevel = targetWind;
            
        }
        else
        {
            if(!warning)
                Warning();
            if(windLevel < 20)
            {
                windLevel += Time.deltaTime;
            }
        }

        windLevelMeter.fillAmount = windLevel / 20;
        if(windLevel >= 19)
        {
            if(RootController.Instance.rootTrail.manualPoints.Count > 20)
            {
                // TODO: add root retracting
                if(!causedDamage)
                {
                    EventsManager.Instance.WindDamage?.Invoke();
                    causedDamage = true;
                }

            }
        }
    }

    void Warning()
    {
        warning = true;
        warningAlarm.SetActive(true);
        wind.SetActive(true);
        StartCoroutine(WindLength(windChangeInterval));
    }
    
    IEnumerator WindLength(float value)
    {
        yield return new WaitForSeconds(value);
        windBuilding = false;
        warning = false;
        causedDamage = false;
        warningAlarm.SetActive(false);
        wind.SetActive(false);
    }
}
