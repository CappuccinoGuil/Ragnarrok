using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Stat
{
    [SerializeField]
    private UIBar bar;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentVal;

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            maxVal = value;
            bar.fillMax = maxVal;
        }
    }

    public float CurrentVal
    {
        get
        {
            return currentVal;
        }

        set
        {
            currentVal = Mathf.Clamp(value, 0, MaxVal);
            bar.Value = currentVal;
        }
    }

    public void Initialize()
    {
        MaxVal = maxVal;
        CurrentVal = currentVal;
    }

}
