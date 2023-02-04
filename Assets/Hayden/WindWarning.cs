using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindWarning : MonoBehaviour
{

    [SerializeField] Image warningSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = Mathf.Sin(Time.timeSinceLevelLoad * 2);
        Color tempColor = warningSprite.color;
        tempColor.a = alpha;
        warningSprite.color = tempColor;
    }
}
