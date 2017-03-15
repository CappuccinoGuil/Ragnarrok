using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour {

    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image content;

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private Color fullColour;

    [SerializeField]
    private Color lowColour;

    [SerializeField]
    private bool lerpColours;

    public float fillMax { get; set; }

    public float Value
    {
        set
        {
            string[] temp = valueText.text.Split(':');
            valueText.text = temp[0] + ": " + Mathf.Round(value);
            fillAmount = Map(value, 0, fillMax, 0, 1);
        }
    }

	void Start ()
    {
		if(lerpColours)
        {
            content.color = fullColour;
        }
	}
	
	void Update ()
    {
        HandleBar();	
	}

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
        if(lerpColours)
        {
            content.color = Color.Lerp(lowColour, fullColour, fillAmount);
        }

    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
