using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRagnar : MonoBehaviour {

    [SerializeField]
    private Stat steam;

    private bool addSteam = false;
    private bool subSteam = false;

    private void Awake()
    {
        steam.Initialize();
    }

    void Update()
    {
        if(addSteam)
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
