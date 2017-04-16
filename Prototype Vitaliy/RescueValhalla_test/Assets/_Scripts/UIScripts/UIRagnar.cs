using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRagnar : MonoBehaviour {

    [SerializeField]
    private Stat steam;

    [SerializeField]
    float steamRate = 10f;

    [HideInInspector] public bool m_steamLaunched = false;


    private bool addSteam = false;
    private bool subSteam = false;
    private bool m_oneThirdFull;
    private bool m_twoThirdsFull;
    private bool m_threeThirdsFull;

    [HideInInspector] public bool m_steamPickedUp;

    RagnarSteamBlast steamBlast;

    private void Awake()
    {
        steam.Initialize();
        steamBlast = GetComponent<RagnarSteamBlast>();
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
            steam.CurrentVal += steamRate * Time.deltaTime;
        }
        if (subSteam)
        {
            steam.CurrentVal -= steamRate * Time.deltaTime;
        }
        if(m_steamPickedUp)
        {
            steam.CurrentVal += 33.35f;
            m_steamPickedUp = false;
        }

        if(m_oneThirdFull)
        {
            steamBlast.m_launchDistance = 4f;
        } else { steamBlast.m_launchDistance = 0f; }
        if (m_twoThirdsFull)
        {
            steamBlast.m_launchDistance = 6f;
        }
        if (m_threeThirdsFull)
        {
            steamBlast.m_launchDistance = 8f;
        }
        if(m_steamLaunched)
        {
            steam.CurrentVal = 0f;
            m_steamLaunched = false;
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
