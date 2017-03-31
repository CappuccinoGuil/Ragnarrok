using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRagnar : MonoBehaviour {

    [SerializeField]
    private Stat steam;

    private bool addSteam = false;
    private bool subSteam = false;

    public bool m_oneThirdFull;
public bool m_twoThirdsFull;
 public bool m_threeThirdsFull;


    private void Awake()
    {
        steam.Initialize();
    }

    void Update()
    {
        if (steam.CurrentVal >= (steam.MaxVal * 0.33f) && steam.CurrentVal < (steam.MaxVal * 0.66f))
        {
            m_oneThirdFull = true;
        }
        else
        {
            m_oneThirdFull = false;
        }

        if (steam.CurrentVal >= (steam.MaxVal * 0.66f) && steam.CurrentVal < steam.MaxVal)
        {
            m_twoThirdsFull = true;
        }
        else
        {
            m_twoThirdsFull = false;
        }

        if (steam.CurrentVal >= steam.MaxVal)
        {
            m_threeThirdsFull = true;
        }
        else
        {
            m_threeThirdsFull = false;
        }

        if (addSteam)
        {
            steam.CurrentVal += 10 * Time.deltaTime;
        }
        if (subSteam)
        {
            steam.CurrentVal -= 10 * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "AddSteam")
        {
            addSteam = true;
        }
        if (col.tag == "SubSteam")
        {
            subSteam = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "AddSteam")
        {
            addSteam = false;
        }
        if (col.tag == "SubSteam")
        {
            subSteam = false;
        }
    }
}
