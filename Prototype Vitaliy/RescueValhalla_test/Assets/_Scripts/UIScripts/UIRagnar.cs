using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRagnar : MonoBehaviour {

    [SerializeField]
    private Stat steam;

    [SerializeField]
    float steamRate = 25f;

    [Header("Steam Blast Launch Distances")]
    [SerializeField] private float m_oneThirdFullDistance = 4f;
    [SerializeField] private float m_twoThirdsFullDistance = 6f;
    [SerializeField] private float m_threeThirdsFullDistance = 8f;


    [HideInInspector] public bool m_steamBlastLaunched = false;
    [HideInInspector] public bool m_steamSpurtLaunched = false;

    private bool addSteam = false;
    private bool subSteam = false;
    private bool m_oneThirdFull;
    private bool m_twoThirdsFull;
    private bool m_threeThirdsFull;

    [HideInInspector] public bool m_steamPickedUp;

    RagnarSteamBlast m_steamBlast;

    private void Awake()
    {
        steam.Initialize();
        m_steamBlast = GetComponent<RagnarSteamBlast>();
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
            m_steamBlast.m_launchDistance = m_oneThirdFullDistance;
            m_steamBlast.m_switchEffectSteamBlast = 1;
        }
        else
        {
            m_steamBlast.m_launchDistance = 0f;
            m_steamBlast.m_switchEffectSteamBlast = 0;
        }
        if (m_twoThirdsFull)
        {
            m_steamBlast.m_launchDistance = m_twoThirdsFullDistance;
            m_steamBlast.m_switchEffectSteamBlast = 2;
        }
        if (m_threeThirdsFull)
        {
            m_steamBlast.m_launchDistance = m_threeThirdsFullDistance;
            m_steamBlast.m_switchEffectSteamBlast = 3;
        }
        if (m_steamBlastLaunched && steam.CurrentVal >= (steam.MaxVal * 0.33f))
        {
            steam.CurrentVal = 0f;
            m_steamBlastLaunched = false;
        }
        else { m_steamBlastLaunched = false; }
        if(m_steamSpurtLaunched && steam.CurrentVal >= steam.MaxVal * 0.165f)
        {
            steam.CurrentVal -= steam.MaxVal * 0.165f;
            m_steamBlast.m_canSpurt = true;
            m_steamSpurtLaunched = false;
        } else if(m_steamSpurtLaunched && steam.CurrentVal < steam.MaxVal * 0.165f) {
            m_steamBlast.m_canSpurt = false;
            m_steamSpurtLaunched = false; }


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
