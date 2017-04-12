using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour {



    [SerializeField] float lerpSpeed;

    [SerializeField] Image content;

    [SerializeField] Text valueText;

    [SerializeField] Color fullColour;

    [SerializeField] Color lowColour;

    [SerializeField] bool lerpColours;

    private float fillAmount;

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

        if (lerpColours)
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
            if (lerpColours)
            {
                content.color = Color.Lerp(lowColour, fullColour, fillAmount);
            }

    }



    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
